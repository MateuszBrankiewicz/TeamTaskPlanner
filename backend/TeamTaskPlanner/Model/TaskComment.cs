namespace TeamTaskPlanner.Model;

public class TaskComment
{
  public int Id { get; set; }
  public string Content { get; set; } = string.Empty;
  public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
  public DateTime? UpdatedDate { get; set; }
  
  // Relacje
  public int TaskId { get; set; }
  public Task Task { get; set; } = null!;
  
  public int UserId { get; set; }
  public User User { get; set; } = null!;
  
  // Odpowiedzi na komentarze
  public int? ParentCommentId { get; set; }
  public TaskComment? ParentComment { get; set; }
  public List<TaskComment> Replies { get; set; } = new();
}
