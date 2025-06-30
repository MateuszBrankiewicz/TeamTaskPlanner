namespace TeamTaskPlanner.Model;

public class Task
{
  public int Id { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public TaskStatus Status { get; set; } = TaskStatus.Todo;
  public TaskPriority Priority { get; set; } = TaskPriority.Medium;
  public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
  public DateTime? DueDate { get; set; }
  public DateTime? CompletedDate { get; set; }
  
  // Relacje
  public int CreatedByUserId { get; set; }
  public User CreatedByUser { get; set; } = null!;
  
  public int? AssignedToUserId { get; set; }
  public User? AssignedToUser { get; set; }
  
  public int? ProjectId { get; set; }
  public Project? Project { get; set; }
  
  // Komentarze
  public List<TaskComment> Comments { get; set; } = new();
  
  // Attachments
  public List<TaskAttachment> Attachments { get; set; } = new();
}

public enum TaskStatus
{
  Todo = 0,
  InProgress = 1,
  InReview = 2,
  Done = 3,
  Blocked = 4,
  Cancelled = 5
}

public enum TaskPriority
{
  Low = 0,
  Medium = 1,  
  High = 2,
  Critical = 3
}
