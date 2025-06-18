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
    return Ok("Użytkownik zarejestrowany pomyślnie.");
  }
  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
  {
    var result = await _authService.LoginAsync(dto);
    if (result == null)
    {
      return Unauthorized("Invalid email or password");
    }
    Response.Cookies.Append("token", result, new CookieOptions
    {
      HttpOnly = true,
      Secure = true,
      SameSite = SameSiteMode.Strict,
      Expires = DateTimeOffset.UtcNow.AddDays(1)
    });
    Response.Cookies.Append("refreshToken", result, new CookieOptions
    {
      HttpOnly = true,
      Secure = true,
      SameSite = SameSiteMode.Strict,
      Expires = DateTimeOffset.UtcNow.AddDays(7)
    });
    return Ok(result);
  }
}

