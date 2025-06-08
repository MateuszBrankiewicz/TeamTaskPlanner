using Microsoft.EntityFrameworkCore;
using TeamTaskPlanner.Model;

namespace TeamTaskPlanner.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<User> Users { get; set; }
  public DbSet<Role> Roles { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<User>().HasOne(u => u.Role).WithMany(r=>r.Users).HasForeignKey(u=>u.Id).OnDelete(DeleteBehavior.Cascade);
  }

}
