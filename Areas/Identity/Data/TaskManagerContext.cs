using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TaskManager.Models;

namespace TaskManager.Data;

public class TaskManagerContext : IdentityDbContext<User>
{
    public TaskManagerContext(DbContextOptions<TaskManagerContext> options)
        : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Relation Project -> Organization
        builder.Entity<Project>()
            .HasOne(p => p.Organization)
            .WithMany(o => o.Projects)
            .HasForeignKey(p => p.OrganizationId)
            .OnDelete(DeleteBehavior.NoAction);

        // Relation Project -> Administrator (Owner)
        builder.Entity<Project>()
            .HasOne(p => p.Owner)
            .WithMany(a => a.CreatedProjects)
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.NoAction);

        // Relation Organization -> Administrator
        builder.Entity<Organization>()
            .HasOne(o => o.Owner)
            .WithMany(a => a.CreatedOrganizations)
            .HasForeignKey(o => o.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<IdentityUserLogin<string>>().HasKey(x => new { x.LoginProvider, x.ProviderKey });
        builder.Entity<IdentityUserRole<string>>().HasKey(x => new { x.UserId, x.RoleId });
        builder.Entity<IdentityUserToken<string>>().HasKey(x => new { x.UserId, x.LoginProvider, x.Name });
    }
}
