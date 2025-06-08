using Microsoft.AspNetCore.Mvc;
using TeamTaskPlanner.Service;

namespace TeamTaskPlanner.Controller;
[Route("api/[controller]")]
[ApiController]
public class AuthController
{
  private readonly AuthService _authService;

  public AuthController(AuthService authService)
  {
    _authService = authService;
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
  {

  }
}
