using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeamTaskPlanner.Dto;
using TeamTaskPlanner.Service;

namespace TeamTaskPlanner.Controller;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProjectController : ControllerBase
{
  private readonly ProjectService _projectService;

  public ProjectController(ProjectService projectService)
  {
    _projectService = projectService;
  }

  [HttpPost]
  public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto dto)
  {
    var userId = GetCurrentUserId();
    if (userId == null)
    {
      return Unauthorized("User ID not found in token");
    }

    var project = await _projectService.CreateProjectAsync(dto, userId.Value);
    if (project == null)
    {
      return BadRequest("Failed to create project");
    }

    return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetProject(int id)
  {
    var userId = GetCurrentUserId();
    if (userId == null)
    {
      return Unauthorized("User ID not found in token");
    }

    var project = await _projectService.GetProjectByIdAsync(id, userId.Value);
    if (project == null)
    {
      return NotFound("Project not found or access denied");
    }

    return Ok(project);
  }

  [HttpGet]
  public async Task<IActionResult> GetProjects()
  {
    var userId = GetCurrentUserId();
    if (userId == null)
    {
      return Unauthorized("User ID not found in token");
    }

    var projects = await _projectService.GetUserProjectsAsync(userId.Value);
    return Ok(projects);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateProject(int id, [FromBody] UpdateProjectDto dto)
  {
    var userId = GetCurrentUserId();
    if (userId == null)
    {
      return Unauthorized("User ID not found in token");
    }

    var project = await _projectService.UpdateProjectAsync(id, dto, userId.Value);
    if (project == null)
    {
      return NotFound("Project not found or access denied");
    }

    return Ok(project);
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteProject(int id)
  {
    var userId = GetCurrentUserId();
    if (userId == null)
    {
      return Unauthorized("User ID not found in token");
    }

    var success = await _projectService.DeleteProjectAsync(id, userId.Value);
    if (!success)
    {
      return NotFound("Project not found or access denied");
    }

    return NoContent();
  }

  // Członkowie projektu
  [HttpPost("{id}/members")]
  public async Task<IActionResult> AddMember(int id, [FromBody] AddProjectMemberDto dto)
  {
    var userId = GetCurrentUserId();
    if (userId == null)
    {
      return Unauthorized("User ID not found in token");
    }

    var member = await _projectService.AddMemberAsync(id, dto, userId.Value);
    if (member == null)
    {
      return BadRequest("Failed to add member. Check if user exists and is not already a member.");
    }

    return Ok(member);
  }

  [HttpGet("{id}/members")]
  public async Task<IActionResult> GetMembers(int id)
  {
    var userId = GetCurrentUserId();
    if (userId == null)
    {
      return Unauthorized("User ID not found in token");
    }

    var members = await _projectService.GetProjectMembersAsync(id, userId.Value);
    return Ok(members);
  }

  [HttpDelete("{id}/members/{memberId}")]
  public async Task<IActionResult> RemoveMember(int id, int memberId)
  {
    var userId = GetCurrentUserId();
    if (userId == null)
    {
      return Unauthorized("User ID not found in token");
    }

    var success = await _projectService.RemoveMemberAsync(id, memberId, userId.Value);
    if (!success)
    {
      return NotFound("Member not found or access denied");
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
