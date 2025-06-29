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
      return BadRequest(new {
    success = false,
    message = "Email is taken",
    errorCode = "EMAIL_ALREADY_EXISTS",
    field = "email"
});
    }
    return Ok(new { message = "Użytkownik zarejestrowany pomyślnie." });
  }
  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
  {
    var result = await _authService.LoginAsync(dto);
    if (result == null)
    {
      return Unauthorized("Invalid email or password");
    }
    Response.Cookies.Append("token", result.AccessToken, new CookieOptions
    {
      HttpOnly = true,
      Secure = true,
      SameSite = SameSiteMode.Strict,
      Expires = DateTimeOffset.UtcNow.AddDays(1)
    });
    Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
    {
      HttpOnly = true,
      Secure = true,
      SameSite = SameSiteMode.Strict,
      Expires = DateTimeOffset.UtcNow.AddDays(7)
    });
    return Ok(new { email = result.Email });
  }

  [HttpPost("refresh-token")]
  public async Task<IActionResult> RefreshToken()
  {
    // Pobierz refresh token z body lub z ciasteczka
    var refreshToken = Request.Cookies["refreshToken"];
    Console.WriteLine(refreshToken);
    if (string.IsNullOrEmpty(refreshToken))
    {
      return BadRequest("Refresh token is required");
    }

    var result = await _authService.RefreshTokenAsync(refreshToken);
    if (result == null)
    {
      return Unauthorized("Invalid or expired refresh token");
    }

    Response.Cookies.Append("token", result.AccessToken, new CookieOptions
    {
      HttpOnly = true,
      Secure = true,
      SameSite = SameSiteMode.Strict,
      Expires = DateTimeOffset.UtcNow.AddDays(1)
    });
    Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
    {
      HttpOnly = true,
      Secure = true,
      SameSite = SameSiteMode.Strict,
      Expires = DateTimeOffset.UtcNow.AddDays(7)
    });

    return Ok(new { email = result.Email });
  }

  [HttpPost("logout")]
  public async Task<IActionResult> Logout()
  {
    var refreshToken = Request.Cookies["refreshToken"];

    if (!string.IsNullOrEmpty(refreshToken))
    {
      await _authService.RevokeRefreshTokenAsync(refreshToken);
    }

    Response.Cookies.Delete("token");
    Response.Cookies.Delete("refreshToken");

    return Ok(new { message = "Logged out successfully" });
  }
}

