using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeamTaskPlanner.Service;

namespace TeamTaskPlanner.Controller;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AdminController : ControllerBase
{
  private readonly DashboardService _dashboardService;

  public AdminController(DashboardService dashboardService)
  {
    _dashboardService = dashboardService;
  }

  [HttpGet("users")]
  [Authorize(Roles = "Admin")]
  public IActionResult GetAllUsers()
  {
    // Tylko administratorzy mogą zobaczyć listę wszystkich użytkowników
    var users = new[]
    {
      new { Id = 1, Email = "admin@example.com", Role = "Admin", LastLogin = DateTime.UtcNow.AddDays(-1) },
      new { Id = 2, Email = "user@example.com", Role = "User", LastLogin = DateTime.UtcNow.AddHours(-3) },
      new { Id = 3, Email = "manager@example.com", Role = "Manager", LastLogin = DateTime.UtcNow.AddMinutes(-30) }
    };

    return Ok(new { users, totalCount = users.Length });
  }

  [HttpGet("system-stats")]
  [Authorize(Roles = "Admin,Manager")]
  public IActionResult GetSystemStats()
  {
    // Administratorzy i menadżerowie mogą zobaczyć statystyki systemu
    var stats = new
    {
      TotalUsers = Random.Shared.Next(100, 1000),
      ActiveUsers = Random.Shared.Next(50, 200),
      TotalTasks = Random.Shared.Next(500, 2000),
      SystemUptime = TimeSpan.FromDays(Random.Shared.Next(1, 30)),
      DatabaseSize = $"{Random.Shared.Next(100, 500)} MB",
      LastBackup = DateTime.UtcNow.AddHours(-Random.Shared.Next(1, 24))
    };

    return Ok(stats);
  }

  [HttpGet("user-activity/{userId}")]
  [Authorize(Roles = "Admin,Manager")]
  public async Task<IActionResult> GetUserActivity(int userId)
  {
    // Sprawdź czy użytkownik próbuje dostać się do swoich danych lub czy ma odpowiednią rolę
    var currentUserId = GetCurrentUserId();
    var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

    if (currentUserId != userId && currentUserRole != "Admin" && currentUserRole != "Manager")
    {
      return Forbid("You can only access your own activity data");
    }

    var activity = new
    {
      UserId = userId,
      TasksCompleted = Random.Shared.Next(10, 50),
      HoursWorked = Random.Shared.Next(20, 80),
      ProjectsInvolved = Random.Shared.Next(2, 10),
      LastActivity = DateTime.UtcNow.AddMinutes(-Random.Shared.Next(1, 1440)),
      RecentActions = new[]
      {
        new { Action = "Completed task", Timestamp = DateTime.UtcNow.AddMinutes(-30) },
        new { Action = "Created new task", Timestamp = DateTime.UtcNow.AddHours(-2) },
        new { Action = "Updated project", Timestamp = DateTime.UtcNow.AddHours(-5) }
      }
    };

    return Ok(activity);
  }

  [HttpPost("maintenance-mode")]
  [Authorize(Roles = "Admin")]
  public IActionResult ToggleMaintenanceMode([FromBody] MaintenanceRequest request)
  {
    // Tylko administratorzy mogą włączać/wyłączać tryb konserwacji
    return Ok(new
    {
      maintenanceMode = request.Enable,
      message = request.Enable ? "Maintenance mode enabled" : "Maintenance mode disabled",
      scheduledFor = request.ScheduledTime,
      enabledBy = User.FindFirst(ClaimTypes.Name)?.Value
    });
  }

  private int? GetCurrentUserId()
  {
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (int.TryParse(userIdClaim, out int userId))
    {
      return userId;
    }
    return null;
  }
}

public class MaintenanceRequest
{
  public bool Enable { get; set; }
  public DateTime? ScheduledTime { get; set; }
  public string Reason { get; set; } = string.Empty;
}
