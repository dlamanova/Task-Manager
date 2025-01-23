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

            // Configure TPH with Discriminator
            builder.Entity<User>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<User>("User")
                .HasValue<RegularUser>("RegularUser")
                .HasValue<Administrator>("Administrator");

            // Seed roles
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "admin-role-id", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "user-role-id", Name = "User", NormalizedName = "USER" }
            );

            // Seed admin user
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

            var passwordHasher = new PasswordHasher<User>();
            admin.PasswordHash = passwordHasher.HashPassword(admin, "Admin123!");
            builder.Entity<Administrator>().HasData(admin);

            // Seed regular user
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

            regularUser.PasswordHash = passwordHasher.HashPassword(regularUser, "User123!");
            builder.Entity<RegularUser>().HasData(regularUser);

            // Assign roles
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "admin-id",
                    RoleId = "admin-role-id"
                },
                new IdentityUserRole<string>
                {
                    UserId = "user-id",
                    RoleId = "user-role-id"
                }
            );

            // Seed example tasks and projects
            builder.Entity<TaskItem>().HasData(
                new TaskItem { Id = 1, Name = "Task 1", AssignedUserId = "user-id", Description = "Smth" },
                new TaskItem { Id = 2, Name = "Admin Task 1", AssignedUserId = "admin-id",
                    Description = "Smth" }
            );

            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Home" },
                new Category { Id = 2, Name = "Work" },
                new Category { Id = 3, Name = "School" }
            );

            //builder.Entity<Project>().HasData(
            //    new Project { Id = 1, Name = "Admin Project 1", OwnerId = "admin-id" }
            //);
        }

    }
}
