using Microsoft.EntityFrameworkCore;
using TeamTaskPlanner.Data;
using TeamTaskPlanner.Dto;
using TeamTaskPlanner.Model;

namespace TeamTaskPlanner.Service;

public class TaskService
{
  private readonly AppDbContext db;

  public TaskService(AppDbContext db)
  {
    this.db = db;
  }

  public async Task<TaskResponseDto?> CreateTaskAsync(CreateTaskDto dto, int createdByUserId)
  {
    // Sprawdź czy assigned user istnieje
    if (dto.AssignedToUserId.HasValue)
    {
      var assignedUser = await db.Users.FindAsync(dto.AssignedToUserId.Value);
      if (assignedUser == null)
      {
        return null;
      }
    }

    // Sprawdź czy projekt istnieje i użytkownik ma dostęp
    if (dto.ProjectId.HasValue)
    {
      var project = await db.Projects
          .Include(p => p.Members)
          .FirstOrDefaultAsync(p => p.Id == dto.ProjectId.Value);

      if (project == null || (!project.Members.Any(m => m.UserId == createdByUserId) && project.CreatedByUserId != createdByUserId))
      {
        return null;
      }
    }

    var task = new Model.Task
    {
      Title = dto.Title,
      Description = dto.Description,
      Priority = (TaskPriority)dto.Priority,
      DueDate = dto.DueDate,
      CreatedByUserId = createdByUserId,
      AssignedToUserId = dto.AssignedToUserId,
      ProjectId = dto.ProjectId,
      Status = Model.TaskStatus.Todo
    };

    db.Tasks.Add(task);
    await db.SaveChangesAsync();

    return await GetTaskByIdAsync(task.Id);
  }

  public async Task<TaskResponseDto?> GetTaskByIdAsync(int taskId)
  {
    var task = await db.Tasks
        .Include(t => t.CreatedByUser)
        .Include(t => t.AssignedToUser)
        .Include(t => t.Project)
        .Include(t => t.Comments)
        .Include(t => t.Attachments)
        .FirstOrDefaultAsync(t => t.Id == taskId);

    if (task == null) return null;

    return MapToTaskResponseDto(task);
  }

  public async Task<TaskDetailDto?> GetTaskDetailAsync(int taskId, int userId)
  {
    var task = await db.Tasks
        .Include(t => t.CreatedByUser)
        .Include(t => t.AssignedToUser)
        .Include(t => t.Project)
        .Include(t => t.Comments)
            .ThenInclude(c => c.User)
        .Include(t => t.Comments)
            .ThenInclude(c => c.Replies)
            .ThenInclude(r => r.User)
        .Include(t => t.Attachments)
            .ThenInclude(a => a.UploadedByUser)
        .FirstOrDefaultAsync(t => t.Id == taskId);

    if (task == null) return null;

    // Sprawdź dostęp do zadania
    if (!await HasAccessToTaskAsync(task, userId))
    {
      return null;
    }

    return MapToTaskDetailDto(task);
  }

  public async Task<List<TaskResponseDto>> GetUserTasksAsync(int userId, Model.TaskStatus? status = null, int? projectId = null)
  {
    var query = db.Tasks
        .Include(t => t.CreatedByUser)
        .Include(t => t.AssignedToUser)
        .Include(t => t.Project)
        .Include(t => t.Comments)
        .Include(t => t.Attachments)
        .Where(t => t.CreatedByUserId == userId || t.AssignedToUserId == userId);

    if (status.HasValue)
    {
      query = query.Where(t => t.Status == status.Value);
    }

    if (projectId.HasValue)
    {
      query = query.Where(t => t.ProjectId == projectId.Value);
    }

    var tasks = await query.OrderByDescending(t => t.CreatedDate).ToListAsync();
    return tasks.Select(MapToTaskResponseDto).ToList();
  }

  public async Task<TaskResponseDto?> UpdateTaskAsync(int taskId, UpdateTaskDto dto, int userId)
  {
    var task = await db.Tasks
        .Include(t => t.Project)
            .ThenInclude(p => p.Members)
        .FirstOrDefaultAsync(t => t.Id == taskId);

    if (task == null) return null;

    // Sprawdź uprawnienia do edycji
    if (!await CanEditTaskAsync(task, userId))
    {
      return null;
    }

    // Aktualizuj pola
    if (!string.IsNullOrEmpty(dto.Title))
      task.Title = dto.Title;

    if (!string.IsNullOrEmpty(dto.Description))
      task.Description = dto.Description;

    if (dto.Status.HasValue)
    {
      task.Status = (Model.TaskStatus)dto.Status.Value;
      if (task.Status == Model.TaskStatus.Done && task.CompletedDate == null)
      {
        task.CompletedDate = DateTime.UtcNow;
      }
      else if (task.Status != Model.TaskStatus.Done)
      {
        task.CompletedDate = null;
      }
    }

    if (dto.Priority.HasValue)
      task.Priority = (TaskPriority)dto.Priority.Value;

    if (dto.DueDate.HasValue)
      task.DueDate = dto.DueDate.Value;

    if (dto.AssignedToUserId.HasValue)
    {
      // Sprawdź czy użytkownik istnieje
      var assignedUser = await db.Users.FindAsync(dto.AssignedToUserId.Value);
      if (assignedUser != null)
      {
        task.AssignedToUserId = dto.AssignedToUserId.Value;
      }
    }

    await db.SaveChangesAsync();
    return await GetTaskByIdAsync(taskId);
  }

  public async Task<bool> DeleteTaskAsync(int taskId, int userId)
  {
    var task = await db.Tasks.FindAsync(taskId);
    if (task == null) return false;

    // Tylko twórca zadania może je usunąć
    if (task.CreatedByUserId != userId)
    {
      return false;
    }

    db.Tasks.Remove(task);
    await db.SaveChangesAsync();
    return true;
  }

  private async Task<bool> HasAccessToTaskAsync(Model.Task task, int userId)
  {
    // Twórca i osoba przypisana mają dostęp
    if (task.CreatedByUserId == userId || task.AssignedToUserId == userId)
      return true;

    // Jeśli zadanie należy do projektu, sprawdź czy użytkownik jest członkiem
    if (task.ProjectId.HasValue)
    {
      var isMember = await db.ProjectMembers
          .AnyAsync(pm => pm.ProjectId == task.ProjectId.Value && pm.UserId == userId);
      return isMember;
    }

    return false;
  }

  private async Task<bool> CanEditTaskAsync(Model.Task task, int userId)
  {
    // Twórca może edytować
    if (task.CreatedByUserId == userId)
      return true;

    // Osoba przypisana może edytować status
    if (task.AssignedToUserId == userId)
      return true;

    // Lider projektu może edytować zadania w projekcie
    if (task.ProjectId.HasValue)
    {
      var isProjectLead = await db.ProjectMembers
          .AnyAsync(pm => pm.ProjectId == task.ProjectId.Value &&
                         pm.UserId == userId &&
                         (pm.Role == ProjectRole.Lead || pm.Role == ProjectRole.Manager));
      return isProjectLead;
    }

    return false;
  }

  private TaskResponseDto MapToTaskResponseDto(Model.Task task)
  {
    return new TaskResponseDto
    {
      Id = task.Id,
      Title = task.Title,
      Description = task.Description,
      Status = task.Status.ToString(),
      Priority = task.Priority.ToString(),
      CreatedDate = task.CreatedDate,
      DueDate = task.DueDate,
      CompletedDate = task.CompletedDate,
      CreatedBy = new UserInfoDto { Id = task.CreatedByUser.Id, Email = task.CreatedByUser.Email },
      AssignedTo = task.AssignedToUser != null ? new UserInfoDto { Id = task.AssignedToUser.Id, Email = task.AssignedToUser.Email } : null,
      Project = task.Project != null ? new ProjectInfoDto { Id = task.Project.Id, Name = task.Project.Name } : null,
      CommentsCount = task.Comments.Count,
      AttachmentsCount = task.Attachments.Count
    };
  }

  private TaskDetailDto MapToTaskDetailDto(Model.Task task)
  {
    var baseDto = MapToTaskResponseDto(task);
    return new TaskDetailDto
    {
      Id = baseDto.Id,
      Title = baseDto.Title,
      Description = baseDto.Description,
      Status = baseDto.Status,
      Priority = baseDto.Priority,
      CreatedDate = baseDto.CreatedDate,
      DueDate = baseDto.DueDate,
      CompletedDate = baseDto.CompletedDate,
      CreatedBy = baseDto.CreatedBy,
      AssignedTo = baseDto.AssignedTo,
      Project = baseDto.Project,
      CommentsCount = baseDto.CommentsCount,
      AttachmentsCount = baseDto.AttachmentsCount,
      Comments = MapTaskComments(task.Comments.Where(c => c.ParentCommentId == null).ToList()),
      Attachments = task.Attachments.Select(a => new TaskAttachmentDto
      {
        Id = a.Id,
        FileName = a.FileName,
        OriginalFileName = a.OriginalFileName,
        ContentType = a.ContentType,
        FileSize = a.FileSize,
        UploadedDate = a.UploadedDate,
        UploadedBy = new UserInfoDto { Id = a.UploadedByUser.Id, Email = a.UploadedByUser.Email }
      }).ToList()
    };
  }

  private List<TaskCommentDto> MapTaskComments(List<TaskComment> comments)
  {
    return comments.Select(c => new TaskCommentDto
    {
      Id = c.Id,
      Content = c.Content,
      CreatedDate = c.CreatedDate,
      UpdatedDate = c.UpdatedDate,
      User = new UserInfoDto { Id = c.User.Id, Email = c.User.Email },
      ParentCommentId = c.ParentCommentId,
      Replies = MapTaskComments(c.Replies.ToList())
    }).ToList();
  }
}
