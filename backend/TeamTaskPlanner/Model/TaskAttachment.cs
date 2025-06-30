namespace TeamTaskPlanner.Model;

public class TaskAttachment
{
  public int Id { get; set; }
  public string FileName { get; set; } = string.Empty;
  public string OriginalFileName { get; set; } = string.Empty;
  public string FilePath { get; set; } = string.Empty;
  public string ContentType { get; set; } = string.Empty;
  public long FileSize { get; set; }
  public DateTime UploadedDate { get; set; } = DateTime.UtcNow;
  
  // Relacje
  public int TaskId { get; set; }
  public Task Task { get; set; } = null!;
  
  public int UploadedByUserId { get; set; }
  public User UploadedByUser { get; set; } = null!;
}
