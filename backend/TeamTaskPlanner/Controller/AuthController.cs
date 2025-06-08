using Microsoft.AspNetCore.Mvc;
using TeamTaskPlanner.Dto;
using TeamTaskPlanner.Service;

namespace TeamTaskPlanner.Controller;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
  private readonly AuthService _authService;

  public AuthController(AuthService authService)
  {
    _authService = authService;
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
  {
    var result = await _authService.RegisterAsync(dto);
    if (!result)
    {
      return BadRequest("Email is taken");
    }
    return Ok("Użytkownik zarejestrowany pomyślnie.");  }
}
