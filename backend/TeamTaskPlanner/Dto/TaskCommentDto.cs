namespace TeamTaskPlanner.Dto;

public class CreateTaskCommentDto
{
  public string Content { get; set; } = string.Empty;
  public int? ParentCommentId { get; set; }
}

public class UpdateTaskCommentDto
{
  public string Content { get; set; } = string.Empty;
}

public class TaskCommentDto
{
  public int Id { get; set; }
  public string Content { get; set; } = string.Empty;
  public DateTime CreatedDate { get; set; }
  public DateTime? UpdatedDate { get; set; }

  public UserInfoDto User { get; set; } = null!;
  public int? ParentCommentId { get; set; }
  public List<TaskCommentDto> Replies { get; set; } = new();
}

public class TaskAttachmentDto
{
  public int Id { get; set; }
  public string FileName { get; set; } = string.Empty;
  public string OriginalFileName { get; set; } = string.Empty;
  public string ContentType { get; set; } = string.Empty;
  public long FileSize { get; set; }
  public DateTime UploadedDate { get; set; }
  public UserInfoDto UploadedBy { get; set; } = null!;
}
