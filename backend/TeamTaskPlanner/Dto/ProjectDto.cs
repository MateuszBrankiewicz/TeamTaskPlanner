namespace TeamTaskPlanner.Dto;

public class CreateProjectDto
{
  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public DateTime? DueDate { get; set; }
}

public class UpdateProjectDto
{
  public string? Name { get; set; }
  public string? Description { get; set; }
  public DateTime? DueDate { get; set; }
  public int? Status { get; set; }
}

public class ProjectResponseDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public DateTime CreatedDate { get; set; }
  public DateTime? DueDate { get; set; }
  public string Status { get; set; } = string.Empty;
  
  public UserInfoDto CreatedBy { get; set; } = null!;
  public List<ProjectMemberDto> Members { get; set; } = new();
  
  public int TasksCount { get; set; }
  public int CompletedTasksCount { get; set; }
}

public class ProjectMemberDto
{
  public int Id { get; set; }
  public UserInfoDto User { get; set; } = null!;
  public string Role { get; set; } = string.Empty;
  public DateTime JoinedDate { get; set; }
}

public class AddProjectMemberDto
{
  public int UserId { get; set; }
  public int Role { get; set; } = 0; // Member
}
