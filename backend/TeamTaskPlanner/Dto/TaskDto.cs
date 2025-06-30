namespace TeamTaskPlanner.Dto;

public class CreateTaskDto
{
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public int Priority { get; set; } = 1; // Medium
  public DateTime? DueDate { get; set; }
  public int? AssignedToUserId { get; set; }
  public int? ProjectId { get; set; }
}

public class UpdateTaskDto
{
  public string? Title { get; set; }
  public string? Description { get; set; }
  public int? Status { get; set; }
  public int? Priority { get; set; }
  public DateTime? DueDate { get; set; }
  public int? AssignedToUserId { get; set; }
  public int? ProjectId { get; set; }
}

public class TaskResponseDto
{
  public int Id { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public string Status { get; set; } = string.Empty;
  public string Priority { get; set; } = string.Empty;
  public DateTime CreatedDate { get; set; }
  public DateTime? DueDate { get; set; }
  public DateTime? CompletedDate { get; set; }
  
  public UserInfoDto CreatedBy { get; set; } = null!;
  public UserInfoDto? AssignedTo { get; set; }
  public ProjectInfoDto? Project { get; set; }
  
  public int CommentsCount { get; set; }
  public int AttachmentsCount { get; set; }
}

public class TaskDetailDto : TaskResponseDto
{
  public List<TaskCommentDto> Comments { get; set; } = new();
  public List<TaskAttachmentDto> Attachments { get; set; } = new();
}

public class UserInfoDto
{
  public int Id { get; set; }
  public string Email { get; set; } = string.Empty;
}

public class ProjectInfoDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
}
