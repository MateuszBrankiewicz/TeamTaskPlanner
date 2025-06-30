using Microsoft.EntityFrameworkCore;
using TeamTaskPlanner.Data;
using TeamTaskPlanner.Dto;
using TeamTaskPlanner.Model;

namespace TeamTaskPlanner.Service;

public class ProjectService
{
  private readonly AppDbContext db;

  public ProjectService(AppDbContext db)
  {
    this.db = db;
  }

  public async Task<ProjectResponseDto?> CreateProjectAsync(CreateProjectDto dto, int createdByUserId)
  {
    var project = new Project
    {
      Name = dto.Name,
      Description = dto.Description,
      DueDate = dto.DueDate,
      CreatedByUserId = createdByUserId,
      Status = ProjectStatus.Active
    };

    db.Projects.Add(project);
    await db.SaveChangesAsync();

    // Dodaj twórcy jako manager projektu
    var projectMember = new ProjectMember
    {
      ProjectId = project.Id,
      UserId = createdByUserId,
      Role = ProjectRole.Manager
    };

    db.ProjectMembers.Add(projectMember);
    await db.SaveChangesAsync();

    return await GetProjectByIdAsync(project.Id, createdByUserId);
  }

  public async Task<ProjectResponseDto?> GetProjectByIdAsync(int projectId, int userId)
  {
    var project = await db.Projects
        .Include(p => p.CreatedByUser)
        .Include(p => p.Members)
            .ThenInclude(m => m.User)
        .Include(p => p.Tasks)
        .FirstOrDefaultAsync(p => p.Id == projectId);

    if (project == null) return null;

    // Sprawdź dostęp do projektu
    if (!HasAccessToProject(project, userId))
    {
      return null;
    }

    return MapToProjectResponseDto(project);
  }

  public async Task<List<ProjectResponseDto>> GetUserProjectsAsync(int userId)
  {
    var projects = await db.Projects
        .Include(p => p.CreatedByUser)
        .Include(p => p.Members)
            .ThenInclude(m => m.User)
        .Include(p => p.Tasks)
        .Where(p => p.CreatedByUserId == userId || p.Members.Any(m => m.UserId == userId))
        .OrderByDescending(p => p.CreatedDate)
        .ToListAsync();

    return projects.Select(MapToProjectResponseDto).ToList();
  }

  public async Task<ProjectResponseDto?> UpdateProjectAsync(int projectId, UpdateProjectDto dto, int userId)
  {
    var project = await db.Projects
        .Include(p => p.Members)
        .FirstOrDefaultAsync(p => p.Id == projectId);

    if (project == null) return null;

    // Sprawdź uprawnienia do edycji (twórca lub manager)
    if (!CanEditProject(project, userId))
    {
      return null;
    }

    if (!string.IsNullOrEmpty(dto.Name))
      project.Name = dto.Name;
      
    if (!string.IsNullOrEmpty(dto.Description))
      project.Description = dto.Description;
      
    if (dto.DueDate.HasValue)
      project.DueDate = dto.DueDate.Value;
      
    if (dto.Status.HasValue)
      project.Status = (ProjectStatus)dto.Status.Value;

    await db.SaveChangesAsync();
    return await GetProjectByIdAsync(projectId, userId);
  }

  public async Task<bool> DeleteProjectAsync(int projectId, int userId)
  {
    var project = await db.Projects.FindAsync(projectId);
    if (project == null) return false;

    // Tylko twórca projektu może go usunąć
    if (project.CreatedByUserId != userId)
    {
      return false;
    }

    db.Projects.Remove(project);
    await db.SaveChangesAsync();
    return true;
  }

  public async Task<ProjectMemberDto?> AddMemberAsync(int projectId, AddProjectMemberDto dto, int userId)
  {
    var project = await db.Projects
        .Include(p => p.Members)
        .FirstOrDefaultAsync(p => p.Id == projectId);

    if (project == null) return null;

    // Sprawdź uprawnienia (twórca lub manager)
    if (!CanManageProjectMembers(project, userId))
    {
      return null;
    }

    // Sprawdź czy użytkownik już nie jest członkiem
    if (project.Members.Any(m => m.UserId == dto.UserId))
    {
      return null;
    }

    // Sprawdź czy użytkownik istnieje
    var user = await db.Users.FindAsync(dto.UserId);
    if (user == null) return null;

    var member = new ProjectMember
    {
      ProjectId = projectId,
      UserId = dto.UserId,
      Role = (ProjectRole)dto.Role
    };

    db.ProjectMembers.Add(member);
    await db.SaveChangesAsync();

    return new ProjectMemberDto
    {
      Id = member.Id,
      User = new UserInfoDto { Id = user.Id, Email = user.Email },
      Role = member.Role.ToString(),
      JoinedDate = member.JoinedDate
    };
  }

  public async Task<bool> RemoveMemberAsync(int projectId, int memberId, int userId)
  {
    var project = await db.Projects
        .Include(p => p.Members)
        .FirstOrDefaultAsync(p => p.Id == projectId);

    if (project == null) return false;

    var member = project.Members.FirstOrDefault(m => m.Id == memberId);
    if (member == null) return false;

    // Sprawdź uprawnienia (twórca, manager lub sam użytkownik)
    if (!CanManageProjectMembers(project, userId) && member.UserId != userId)
    {
      return false;
    }

    // Nie można usunąć twórcy projektu
    if (member.UserId == project.CreatedByUserId)
    {
      return false;
    }

    db.ProjectMembers.Remove(member);
    await db.SaveChangesAsync();
    return true;
  }

  public async Task<List<ProjectMemberDto>> GetProjectMembersAsync(int projectId, int userId)
  {
    var project = await db.Projects
        .Include(p => p.Members)
            .ThenInclude(m => m.User)
        .FirstOrDefaultAsync(p => p.Id == projectId);

    if (project == null || !HasAccessToProject(project, userId))
    {
      return new List<ProjectMemberDto>();
    }

    return project.Members.Select(m => new ProjectMemberDto
    {
      Id = m.Id,
      User = new UserInfoDto { Id = m.User.Id, Email = m.User.Email },
      Role = m.Role.ToString(),
      JoinedDate = m.JoinedDate
    }).ToList();
  }

  private bool HasAccessToProject(Project project, int userId)
  {
    return project.CreatedByUserId == userId || project.Members.Any(m => m.UserId == userId);
  }

  private bool CanEditProject(Project project, int userId)
  {
    if (project.CreatedByUserId == userId) return true;
    
    var member = project.Members.FirstOrDefault(m => m.UserId == userId);
    return member != null && (member.Role == ProjectRole.Manager || member.Role == ProjectRole.Lead);
  }

  private bool CanManageProjectMembers(Project project, int userId)
  {
    if (project.CreatedByUserId == userId) return true;
    
    var member = project.Members.FirstOrDefault(m => m.UserId == userId);
    return member != null && member.Role == ProjectRole.Manager;
  }

  private ProjectResponseDto MapToProjectResponseDto(Project project)
  {
    var completedTasks = project.Tasks.Count(t => t.Status == Model.TaskStatus.Done);
    
    return new ProjectResponseDto
    {
      Id = project.Id,
      Name = project.Name,
      Description = project.Description,
      CreatedDate = project.CreatedDate,
      DueDate = project.DueDate,
      Status = project.Status.ToString(),
      CreatedBy = new UserInfoDto { Id = project.CreatedByUser.Id, Email = project.CreatedByUser.Email },
      Members = project.Members.Select(m => new ProjectMemberDto
      {
        Id = m.Id,
        User = new UserInfoDto { Id = m.User.Id, Email = m.User.Email },
        Role = m.Role.ToString(),
        JoinedDate = m.JoinedDate
      }).ToList(),
      TasksCount = project.Tasks.Count,
      CompletedTasksCount = completedTasks
    };
  }
}
