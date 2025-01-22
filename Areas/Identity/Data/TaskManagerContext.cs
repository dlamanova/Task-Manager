using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Data
{
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
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed ról
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "admin-role-id", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "user-role-id", Name = "User", NormalizedName = "USER" }
            );

            // Seed administratora
            var admin = new Administrator
            {
                Id = "admin-id",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var adminPasswordHasher = new PasswordHasher<Administrator>();
            admin.PasswordHash = adminPasswordHasher.HashPassword(admin, "Admin123!");

            builder.Entity<Administrator>().HasData(admin);

            // Przypisanie roli administratora
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "admin-id",
                    RoleId = "admin-role-id"
                }
            );

            // Seed regularnego użytkownika (przykład)
            var regularUser = new RegularUser
            {
                Id = "user-id",
                UserName = "regularuser",
                NormalizedUserName = "REGULARUSER",
                Email = "regularuser@example.com",
                NormalizedEmail = "REGULARUSER@EXAMPLE.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var regularUserPasswordHasher = new PasswordHasher<RegularUser>();
            regularUser.PasswordHash = regularUserPasswordHasher.HashPassword(regularUser, "User123!");

            builder.Entity<RegularUser>().HasData(regularUser);

            // Przypisanie roli użytkownika
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "user-id",
                    RoleId = "user-role-id"
                }
            );

            // Seed kategorii
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Home" },
                new Category { Id = 2, Name = "Work" },
                new Category { Id = 3, Name = "School" }
            );

            // Seed statusów
            builder.Entity<Status>().HasData(
                new Status { Id = 1, Name = "TODO" },
                new Status { Id = 2, Name = "In Progress" },
                new Status { Id = 3, Name = "Done" }
            );
        }
    }
}
