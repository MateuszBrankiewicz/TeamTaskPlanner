using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeamTaskPlanner.Dto;
using TeamTaskPlanner.Service;
using TeamTaskPlanner.Model;

namespace TeamTaskPlanner.Controller;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TaskController : ControllerBase
{
  private readonly TaskService _taskService;
  private readonly TaskCommentService _commentService;

  public TaskController(TaskService taskService, TaskCommentService commentService)
  {
    _taskService = taskService;
    _commentService = commentService;
  }

  [HttpPost]
  public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
  {
    var userId = GetCurrentUserId();
    if (userId == null)
    {
      return Unauthorized("User ID not found in token");
    }

    var task = await _taskService.CreateTaskAsync(dto, userId.Value);
    if (task == null)
    {
      return BadRequest("Failed to create task. Check if assigned user and project exist.");
    }

    return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetTask(int id)
  {
    var userId = GetCurrentUserId();
    if (userId == null)
    {
      return Unauthorized("User ID not found in token");
    }

    var task = await _taskService.GetTaskDetailAsync(id, userId.Value);
    if (task == null)
    {
      return NotFound("Task not found or access denied");
    }

    return Ok(task);
  }

  [HttpGet]
  public async Task<IActionResult> GetTasks([FromQuery] string? status, [FromQuery] int? projectId)
  {
    var userId = GetCurrentUserId();
    if (userId == null)
    {
      return Unauthorized("User ID not found in token");
    }

    Model.TaskStatus? taskStatus = null;
    if (!string.IsNullOrEmpty(status) && Enum.TryParse<Model.TaskStatus>(status, true, out var parsedStatus))
    {
      taskStatus = parsedStatus;
    }

    var tasks = await _taskService.GetUserTasksAsync(userId.Value, taskStatus, projectId);
    return Ok(tasks);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto dto)
  {
    var userId = GetCurrentUserId();
    if (userId == null)
    {
      return Unauthorized("User ID not found in token");
    }

    var task = await _taskService.UpdateTaskAsync(id, dto, userId.Value);
    if (task == null)
    {
      return NotFound("Task not found or access denied");
    }

    return Ok(task);
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteTask(int id)
  {
    var userId = GetCurrentUserId();
    if (userId == null)
    {
      return Unauthorized("User ID not found in token");
    }

    var success = await _taskService.DeleteTaskAsync(id, userId.Value);
    if (!success)
    {
      return NotFound("Task not found or access denied");
    }

    return NoContent();
  }

  // Komentarze
  [HttpPost("{id}/comments")]
  public async Task<IActionResult> AddComment(int id, [FromBody] CreateTaskCommentDto dto)
  {
    var userId = GetCurrentUserId();
    if (userId == null)
    {
      return Unauthorized("User ID not found in token");
    }

    var comment = await _commentService.AddCommentAsync(id, dto, userId.Value);
    if (comment == null)
    {
      return BadRequest("Failed to add comment. Check if task exists and you have access.");
    }

    return CreatedAtAction(nameof(GetTaskComments), new { id }, comment);
  }

  [HttpGet("{id}/comments")]
  public async Task<IActionResult> GetTaskComments(int id)
  {
    var userId = GetCurrentUserId();
    if (userId == null)
    {
      return Unauthorized("User ID not found in token");
    }

    var comments = await _commentService.GetTaskCommentsAsync(id, userId.Value);
    return Ok(comments);
  }

  [HttpPut("comments/{commentId}")]
  public async Task<IActionResult> UpdateComment(int commentId, [FromBody] UpdateTaskCommentDto dto)
  {
    var userId = GetCurrentUserId();
    if (userId == null)
    {
      return Unauthorized("User ID not found in token");
    }

    var comment = await _commentService.UpdateCommentAsync(commentId, dto, userId.Value);
    if (comment == null)
    {
      return NotFound("Comment not found or access denied");
    }

    return Ok(comment);
  }

  [HttpDelete("comments/{commentId}")]
  public async Task<IActionResult> DeleteComment(int commentId)
  {
    var userId = GetCurrentUserId();
    if (userId == null)
    {
      return Unauthorized("User ID not found in token");
    }

    var success = await _commentService.DeleteCommentAsync(commentId, userId.Value);
    if (!success)
    {
      return NotFound("Comment not found or access denied");
    }

    return NoContent();
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
