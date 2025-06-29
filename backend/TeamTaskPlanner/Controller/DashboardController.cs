using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeamTaskPlanner.Service;

namespace TeamTaskPlanner.Controller;

[Route("api/[controller]")]
[ApiController]
[Authorize] // Wymaga uwierzytelnienia JWT
public class DashboardController : ControllerBase
{
  private readonly DashboardService _dashboardService;

  public DashboardController(DashboardService dashboardService)
  {
    _dashboardService = dashboardService;
  }

  [HttpGet]
  public async Task<IActionResult> GetDashboard()
  {
    var userId = GetCurrentUserId();
    if (userId == null)
    {
      return Unauthorized("User ID not found in token");
    }

    var dashboardData = await _dashboardService.GetDashboardDataAsync(userId.Value);
    return Ok(dashboardData);
  }

  [HttpGet("tasks")]
  public async Task<IActionResult> GetUserTasks()
  {
    var userId = GetCurrentUserId();
    if (userId == null)
    {
      return Unauthorized("User ID not found in token");
    }

    var tasks = await _dashboardService.GetUserTasksAsync(userId.Value);
    return Ok(tasks);
  }

  [HttpGet("stats")]
  public async Task<IActionResult> GetUserStats()
  {
    var userId = GetCurrentUserId();
    if (userId == null)
    {
      return Unauthorized("User ID not found in token");
    }

    var stats = await _dashboardService.GetUserStatsAsync(userId.Value);
    return Ok(stats);
  }

  [HttpGet("profile")]
  public IActionResult GetUserProfile()
  {
    var userEmail = User.FindFirst(ClaimTypes.Name)?.Value;
    var userId = GetCurrentUserId();
    var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "User";

    return Ok(new
    {
      userId = userId,
      email = userEmail,
      role = userRole,
      isAuthenticated = User.Identity?.IsAuthenticated ?? false,
      claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
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
