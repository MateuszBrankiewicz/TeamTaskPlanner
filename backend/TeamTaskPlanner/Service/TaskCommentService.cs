using Microsoft.EntityFrameworkCore;
using TeamTaskPlanner.Data;
using TeamTaskPlanner.Dto;
using TeamTaskPlanner.Model;

namespace TeamTaskPlanner.Service;

public class TaskCommentService
{
  private readonly AppDbContext db;

  public TaskCommentService(AppDbContext db)
  {
    this.db = db;
  }

  public async Task<TaskCommentDto?> AddCommentAsync(int taskId, CreateTaskCommentDto dto, int userId)
  {
    // Sprawdź czy zadanie istnieje i użytkownik ma dostęp
    var task = await db.Tasks
        .Include(t => t.Project)
            .ThenInclude(p => p.Members)
        .FirstOrDefaultAsync(t => t.Id == taskId);

    if (task == null) return null;

    if (!await HasAccessToTaskAsync(task, userId))
    {
      return null;
    }

    // Jeśli to odpowiedź, sprawdź czy komentarz nadrzędny istnieje
    if (dto.ParentCommentId.HasValue)
    {
      var parentComment = await db.TaskComments
          .FirstOrDefaultAsync(c => c.Id == dto.ParentCommentId.Value && c.TaskId == taskId);
      if (parentComment == null) return null;
    }

    var comment = new TaskComment
    {
      Content = dto.Content,
      TaskId = taskId,
      UserId = userId,
      ParentCommentId = dto.ParentCommentId
    };

    db.TaskComments.Add(comment);
    await db.SaveChangesAsync();

    return await GetCommentByIdAsync(comment.Id);
  }

  public async Task<TaskCommentDto?> UpdateCommentAsync(int commentId, UpdateTaskCommentDto dto, int userId)
  {
    var comment = await db.TaskComments
        .Include(c => c.Task)
            .ThenInclude(t => t.Project)
            .ThenInclude(p => p.Members)
        .FirstOrDefaultAsync(c => c.Id == commentId);

    if (comment == null) return null;

    // Tylko autor komentarza może go edytować
    if (comment.UserId != userId)
    {
      return null;
    }

    comment.Content = dto.Content;
    comment.UpdatedDate = DateTime.UtcNow;

    await db.SaveChangesAsync();
    return await GetCommentByIdAsync(commentId);
  }

  public async Task<bool> DeleteCommentAsync(int commentId, int userId)
  {
    var comment = await db.TaskComments
        .Include(c => c.Task)
            .ThenInclude(t => t.Project)
            .ThenInclude(p => p.Members)
        .FirstOrDefaultAsync(c => c.Id == commentId);

    if (comment == null) return false;

    // Autor komentarza lub twórca zadania może usunąć komentarz
    if (comment.UserId != userId && comment.Task.CreatedByUserId != userId)
    {
      // Sprawdź czy to lider projektu
      if (comment.Task.ProjectId.HasValue)
      {
        var isProjectLead = await db.ProjectMembers
            .AnyAsync(pm => pm.ProjectId == comment.Task.ProjectId.Value && 
                           pm.UserId == userId && 
                           (pm.Role == ProjectRole.Lead || pm.Role == ProjectRole.Manager));
        if (!isProjectLead) return false;
      }
      else
      {
        return false;
      }
    }

    db.TaskComments.Remove(comment);
    await db.SaveChangesAsync();
    return true;
  }

  public async Task<List<TaskCommentDto>> GetTaskCommentsAsync(int taskId, int userId)
  {
    // Sprawdź dostęp do zadania
    var task = await db.Tasks
        .Include(t => t.Project)
            .ThenInclude(p => p.Members)
        .FirstOrDefaultAsync(t => t.Id == taskId);

    if (task == null || !await HasAccessToTaskAsync(task, userId))
    {
      return new List<TaskCommentDto>();
    }

    var comments = await db.TaskComments
        .Include(c => c.User)
        .Include(c => c.Replies)
            .ThenInclude(r => r.User)
        .Where(c => c.TaskId == taskId && c.ParentCommentId == null)
        .OrderBy(c => c.CreatedDate)
        .ToListAsync();

    return MapTaskComments(comments);
  }

  private async Task<TaskCommentDto?> GetCommentByIdAsync(int commentId)
  {
    var comment = await db.TaskComments
        .Include(c => c.User)
        .Include(c => c.Replies)
            .ThenInclude(r => r.User)
        .FirstOrDefaultAsync(c => c.Id == commentId);

    if (comment == null) return null;

    return MapTaskComment(comment);
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

  private List<TaskCommentDto> MapTaskComments(List<TaskComment> comments)
  {
    return comments.Select(MapTaskComment).ToList();
  }

  private TaskCommentDto MapTaskComment(TaskComment comment)
  {
    return new TaskCommentDto
    {
      Id = comment.Id,
      Content = comment.Content,
      CreatedDate = comment.CreatedDate,
      UpdatedDate = comment.UpdatedDate,
      User = new UserInfoDto { Id = comment.User.Id, Email = comment.User.Email },
      ParentCommentId = comment.ParentCommentId,
      Replies = comment.Replies.Select(MapTaskComment).ToList()
    };
  }
}
