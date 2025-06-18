using System.Security.Claims;
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
    var role = await db.Roles.FirstOrDefaultAsync(r => r.Name == "User");
    if (role == null)
    {
      role = new Role { Name = "User" };
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

  internal async Task<string?> LoginAsync(LoginUserDto dto)
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
    return GenerateJwtToken(user);
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
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
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
}
