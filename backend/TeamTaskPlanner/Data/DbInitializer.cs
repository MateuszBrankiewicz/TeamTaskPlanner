using Microsoft.EntityFrameworkCore;
using TeamTaskPlanner.Model;

namespace TeamTaskPlanner.Data;

public class DbInitializer
{
  public static void Initialize(AppDbContext dbContext)
  {
    try
    {
      dbContext.Database.Migrate();

      // Seed roles if they don't exist
      SeedRoles(dbContext);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }

  private static void SeedRoles(AppDbContext context)
  {
    var roles = new[] { "User", "Manager", "Admin" };

    foreach (var roleName in roles)
    {
      if (!context.Roles.Any(r => r.Name == roleName))
      {
        context.Roles.Add(new Role { Name = roleName });
      }
    }

    context.SaveChanges();
  }
}
