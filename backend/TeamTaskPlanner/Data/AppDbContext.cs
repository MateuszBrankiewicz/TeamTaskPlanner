using Microsoft.EntityFrameworkCore;

namespace TeamTaskPlanner.Data;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
}
