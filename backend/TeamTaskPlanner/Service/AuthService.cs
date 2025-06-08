using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
      role = new Role{Name = "User"};
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
}
