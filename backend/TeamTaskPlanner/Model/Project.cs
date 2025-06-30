namespace TeamTaskPlanner.Model;

public class Project
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
  public DateTime? DueDate { get; set; }
  public ProjectStatus Status { get; set; } = ProjectStatus.Active;
  
  // Relacje
  public int CreatedByUserId { get; set; }
  public User CreatedByUser { get; set; } = null!;
  
  public List<Task> Tasks { get; set; } = new();
  public List<ProjectMember> Members { get; set; } = new();
}

public class ProjectMember
{
  public int Id { get; set; }
  public int ProjectId { get; set; }
  public Project Project { get; set; } = null!;
  
  public int UserId { get; set; }
  public User User { get; set; } = null!;
  
  public ProjectRole Role { get; set; } = ProjectRole.Member;
  public DateTime JoinedDate { get; set; } = DateTime.UtcNow;
}

public enum ProjectStatus
{
  Active = 0,
  OnHold = 1,
  Completed = 2,
  Cancelled = 3
}

public enum ProjectRole
{
  Member = 0,
  Lead = 1,
  Manager = 2
}
