namespace TeamTaskPlanner.Model;

public class User
{
  public int Id { get; set; }
  public string Email { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
  public int RoleId { get; set; }
  public Role? Role { get; set; }
  public string? RefreshToken { get; set; }
  public DateTime? RefreshTokenExpiry { get; set; }

  // Relacje z zadaniami
  public List<Task> CreatedTasks { get; set; } = new();
  public List<Task> AssignedTasks { get; set; } = new();
  public List<TaskComment> TaskComments { get; set; } = new();
  public List<TaskAttachment> TaskAttachments { get; set; } = new();

  // Relacje z projektami
  public List<Project> CreatedProjects { get; set; } = new();
  public List<ProjectMember> ProjectMemberships { get; set; } = new();
}
