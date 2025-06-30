using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TeamTaskPlanner.Data;
using TeamTaskPlanner.Dto;
using TeamTaskPlanner.Model;

namespace TeamTaskPlanner.Service;

public class LoginResult
{
  public string AccessToken { get; set; } = string.Empty;
  public string RefreshToken { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
}

public class AuthService
{
  private readonly AppDbContext db;
  private readonly IPasswordHasher<User> hasher;
  private readonly IConfiguration configuration;

  public AuthService(AppDbContext db, IPasswordHasher<User> hasher, IConfiguration configuration)
  {
    this.db = db;
    this.hasher = hasher;
    this.configuration = configuration;
  }

  public async Task<bool> RegisterAsync(RegisterUserDto dto)
  {
    if (await db.Users.AnyAsync(u => u.Email == dto.Email))
    {
      return false;
    }

    // Determine role based on email domain or if it's the first user
    string roleName = "User";
    if (dto.Email.Contains("admin@") || dto.Email.StartsWith("admin"))
    {
      roleName = "Admin";
    }
    else if (dto.Email.Contains("manager@") || dto.Email.StartsWith("manager"))
    {
      roleName = "Manager";
    }

    var role = await db.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
    if (role == null)
    {
      role = new Role { Name = roleName };
      db.Roles.Add(role);
      await db.SaveChangesAsync();
    }

    var user = new User
    {
      Email = dto.Email,
      RoleId = role.Id,
    };
    user.Password = this.hasher.HashPassword(user, password: dto.Password);
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return true;
  }

  internal async Task<LoginResult?> LoginAsync(LoginUserDto dto)
  {
    var user = await db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == dto.Email);
    if (user == null)
    {
      return null;

    }
    var result = hasher.VerifyHashedPassword(user, user.Password, dto.Password);
    if (result == PasswordVerificationResult.Failed)
    {
      return null;
    }
    var accessToken = GenerateJwtToken(user);
    var refreshToken = GenerateRefreshToken();

    // Zapisz refresh token w bazie danych
    user.RefreshToken = refreshToken;
    user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
    await db.SaveChangesAsync();

    return new LoginResult
    {
      AccessToken = accessToken ?? string.Empty,
      RefreshToken = refreshToken,
      Email = user.Email
    };
  }

  private string? GenerateJwtToken(User user)
  {
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
    };
    if (user.Role != null && !string.IsNullOrEmpty(user.Role.Name))
    {
      claims.Add(new Claim(ClaimTypes.Role, user.Role.Name));
    }
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret not configured")));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        issuer: configuration["Jwt:Issuer"],
        audience: configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddHours(1),
        signingCredentials: creds
    );
    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  private string GenerateRefreshToken()
  {
    using (var rng = RandomNumberGenerator.Create())
    {
      var randomBytes = new byte[64];
      rng.GetBytes(randomBytes);
      return Convert.ToBase64String(randomBytes);
    }
  }

  public async Task<LoginResult?> RefreshTokenAsync(string refreshToken)
  {
    var user = await db.Users.Include(u => u.Role)
        .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiry > DateTime.UtcNow);

    if (user == null)
    {
      return null;
    }

    var newAccessToken = GenerateJwtToken(user);
    var newRefreshToken = GenerateRefreshToken();

    user.RefreshToken = newRefreshToken;
    user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

    await db.SaveChangesAsync();

    return new LoginResult
    {
      AccessToken = newAccessToken ?? string.Empty,
      RefreshToken = newRefreshToken,
      Email = user.Email
    };
  }

  public async System.Threading.Tasks.Task RevokeRefreshTokenAsync(string refreshToken)
  {
    var user = await db.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
    if (user != null)
    {
      user.RefreshToken = null;
      user.RefreshTokenExpiry = null;
      await db.SaveChangesAsync();
    }
  }
}
