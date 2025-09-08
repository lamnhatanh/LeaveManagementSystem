using LeaveManagement.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet for your LeaveType table
        public DbSet<LeaveType> LeaveTypes => Set<LeaveType>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1. Seed Roles
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "employee",
                    NormalizedName = "EMPLOYEE"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "supervisor",
                    NormalizedName = "SUPERVISOR"
                },
                new IdentityRole
                {
                    Id = "3",
                    Name = "administrator",
                    NormalizedName = "ADMINISTRATOR"
                }
            );

            // 2. Seed User
            var hasher = new PasswordHasher<IdentityUser>();
            var adminUser = new IdentityUser
            {
                Id = "100", // fixed Id for seeding
                UserName = "admin@local.com",
                NormalizedUserName = "ADMIN@LOCAL.COM",
                Email = "admin@local.com",
                NormalizedEmail = "ADMIN@LOCAL.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            // Hash password
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin@123");

            builder.Entity<IdentityUser>().HasData(adminUser);

            // 3. Seed UserRole (map user to Administrator role)
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "100",
                    RoleId = "3" // administrator
                }
            );
        }
    }
}
