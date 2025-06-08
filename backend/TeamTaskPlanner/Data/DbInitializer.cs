using Microsoft.EntityFrameworkCore;

namespace TeamTaskPlanner.Data;

public class DbInitializer
{
  public static void Initialize(AppDbContext dbContext)
  {
    try
    {
dbContext.Database.Migrate();
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }
}
