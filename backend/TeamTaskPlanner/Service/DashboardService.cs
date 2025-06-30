using TeamTaskPlanner.Model;

namespace TeamTaskPlanner.Service;

public class DashboardService
{
  public async System.Threading.Tasks.Task<DashboardData> GetDashboardDataAsync(int userId)
  {
    // Symulacja pobierania danych - zwracamy przykładowe dane
    await System.Threading.Tasks.Task.Delay(100); // Symulacja opóźnienia bazy danych

    return new DashboardData
    {
      TotalTasks = Random.Shared.Next(10, 50),
      CompletedTasks = Random.Shared.Next(5, 25),
      PendingTasks = Random.Shared.Next(3, 15),
      RecentTasks = GenerateRandomTasks(),
      UserName = $"User_{userId}",
      LastLoginDate = DateTime.UtcNow.AddDays(-Random.Shared.Next(0, 7))
    };
  }
  public async System.Threading.Tasks.Task<List<TaskSummary>> GetUserTasksAsync(int userId)
  {
    await System.Threading.Tasks.Task.Delay(50);
    return GenerateRandomTasks();
  }

  public async System.Threading.Tasks.Task<UserStats> GetUserStatsAsync(int userId)
  {
    await System.Threading.Tasks.Task.Delay(30);

    return new UserStats
    {
      TasksCompletedThisWeek = Random.Shared.Next(1, 10),
      TasksCompletedThisMonth = Random.Shared.Next(5, 30),
      AverageTaskCompletionTime = TimeSpan.FromHours(Random.Shared.Next(1, 24)),
      ProductivityScore = Random.Shared.Next(60, 100)
    };
  }

  private List<TaskSummary> GenerateRandomTasks()
  {
    var tasks = new List<TaskSummary>();
    var taskNames = new[]
    {
      "Implement user authentication",
      "Design database schema",
      "Create API endpoints",
      "Write unit tests",
      "Update documentation",
      "Fix bug in login system",
      "Optimize database queries",
      "Deploy to production"
    };

    var priorities = new[] { "Low", "Medium", "High", "Critical" };
    var statuses = new[] { "Todo", "In Progress", "Done", "Blocked" };

    for (int i = 0; i < Random.Shared.Next(3, 8); i++)
    {
      tasks.Add(new TaskSummary
      {
        Id = i + 1,
        Title = taskNames[Random.Shared.Next(taskNames.Length)],
        Description = $"Task description for task {i + 1}",
        Priority = priorities[Random.Shared.Next(priorities.Length)],
        Status = statuses[Random.Shared.Next(statuses.Length)],
        CreatedDate = DateTime.UtcNow.AddDays(-Random.Shared.Next(0, 30)),
        DueDate = DateTime.UtcNow.AddDays(Random.Shared.Next(1, 14))
      });
    }

    return tasks;
  }
}

public class DashboardData
{
  public int TotalTasks { get; set; }
  public int CompletedTasks { get; set; }
  public int PendingTasks { get; set; }
  public List<TaskSummary> RecentTasks { get; set; } = new();
  public string UserName { get; set; } = string.Empty;
  public DateTime LastLoginDate { get; set; }
}

public class TaskSummary
{
  public int Id { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public string Priority { get; set; } = string.Empty;
  public string Status { get; set; } = string.Empty;
  public DateTime CreatedDate { get; set; }
  public DateTime DueDate { get; set; }
}

public class UserStats
{
  public int TasksCompletedThisWeek { get; set; }
  public int TasksCompletedThisMonth { get; set; }
  public TimeSpan AverageTaskCompletionTime { get; set; }
  public int ProductivityScore { get; set; }
}
