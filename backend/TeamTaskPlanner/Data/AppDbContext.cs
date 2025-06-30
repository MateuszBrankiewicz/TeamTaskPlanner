using Microsoft.EntityFrameworkCore;
using TeamTaskPlanner.Model;

namespace TeamTaskPlanner.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<User> Users { get; set; }
  public DbSet<Role> Roles { get; set; }
  
  // Tasks
  public DbSet<Model.Task> Tasks { get; set; }
  public DbSet<TaskComment> TaskComments { get; set; }
  public DbSet<TaskAttachment> TaskAttachments { get; set; }
  
  // Projects
  public DbSet<Project> Projects { get; set; }
  public DbSet<ProjectMember> ProjectMembers { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    
    // User relationships
    modelBuilder.Entity<User>()
        .HasOne(u => u.Role)
        .WithMany(r => r.Users)
        .HasForeignKey(u => u.RoleId)
        .OnDelete(DeleteBehavior.Cascade);
    
    // Task relationships
    modelBuilder.Entity<Model.Task>()
        .HasOne(t => t.CreatedByUser)
        .WithMany(u => u.CreatedTasks)
        .HasForeignKey(t => t.CreatedByUserId)
        .OnDelete(DeleteBehavior.Restrict);
        
    modelBuilder.Entity<Model.Task>()
        .HasOne(t => t.AssignedToUser)
        .WithMany(u => u.AssignedTasks)
        .HasForeignKey(t => t.AssignedToUserId)
        .OnDelete(DeleteBehavior.SetNull);
        
    modelBuilder.Entity<Model.Task>()
        .HasOne(t => t.Project)
        .WithMany(p => p.Tasks)
        .HasForeignKey(t => t.ProjectId)
        .OnDelete(DeleteBehavior.SetNull);
    
    // TaskComment relationships
    modelBuilder.Entity<TaskComment>()
        .HasOne(tc => tc.Task)
        .WithMany(t => t.Comments)
        .HasForeignKey(tc => tc.TaskId)
        .OnDelete(DeleteBehavior.Cascade);
        
    modelBuilder.Entity<TaskComment>()
        .HasOne(tc => tc.User)
        .WithMany(u => u.TaskComments)
        .HasForeignKey(tc => tc.UserId)
        .OnDelete(DeleteBehavior.Restrict);
        
    modelBuilder.Entity<TaskComment>()
        .HasOne(tc => tc.ParentComment)
        .WithMany(tc => tc.Replies)
        .HasForeignKey(tc => tc.ParentCommentId)
        .OnDelete(DeleteBehavior.Restrict);
    
    // TaskAttachment relationships
    modelBuilder.Entity<TaskAttachment>()
        .HasOne(ta => ta.Task)
        .WithMany(t => t.Attachments)
        .HasForeignKey(ta => ta.TaskId)
        .OnDelete(DeleteBehavior.Cascade);
        
    modelBuilder.Entity<TaskAttachment>()
        .HasOne(ta => ta.UploadedByUser)
        .WithMany(u => u.TaskAttachments)
        .HasForeignKey(ta => ta.UploadedByUserId)
        .OnDelete(DeleteBehavior.Restrict);
    
    // Project relationships
    modelBuilder.Entity<Project>()
        .HasOne(p => p.CreatedByUser)
        .WithMany(u => u.CreatedProjects)
        .HasForeignKey(p => p.CreatedByUserId)
        .OnDelete(DeleteBehavior.Restrict);
    
    // ProjectMember relationships
    modelBuilder.Entity<ProjectMember>()
        .HasOne(pm => pm.Project)
        .WithMany(p => p.Members)
        .HasForeignKey(pm => pm.ProjectId)
        .OnDelete(DeleteBehavior.Cascade);
        
    modelBuilder.Entity<ProjectMember>()
        .HasOne(pm => pm.User)
        .WithMany(u => u.ProjectMemberships)
        .HasForeignKey(pm => pm.UserId)
        .OnDelete(DeleteBehavior.Cascade);
        
    // Unique constraints
    modelBuilder.Entity<ProjectMember>()
        .HasIndex(pm => new { pm.ProjectId, pm.UserId })
        .IsUnique();
  }
}
