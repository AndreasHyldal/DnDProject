using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Worktime> Worktimes { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Seed Employee (Test User)
            var testEmployee = new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PasswordHash = HashPassword("Test123!"), // Hash the password
                Role = "Employee",
                DateOfBirth = new DateTime(1990, 5, 15),
                HireDate = new DateTime(2024, 2, 1) // ✅ Static value instead of DateTime.UtcNow
            };

            // ✅ Seed Employee (Test Admin User)
            var adminTestEmployee = new Employee
            {
                Id = 2, // Must be unique
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                PasswordHash = HashPassword("Test123!"), // Hash the password
                Role = "Admin",
                DateOfBirth = new DateTime(1985, 10, 20),
                HireDate = new DateTime(2023, 5, 5) // ✅ Static value instead of DateTime.UtcNow
            };

            // ✅ Seed Worktime Entries (Only FK is set)
            var worktimes = new List<Worktime>
            {
                new Worktime
                {
                    Id = 1,
                    EmployeeId = testEmployee.Id, // Use FK only
                    Start = new DateTime(2024, 2, 1, 9, 0, 0),
                    End = new DateTime(2024, 2, 1, 17, 0, 0),
                    Task = "Worked on frontend UI"
                },
                new Worktime
                {
                    Id = 2,
                    EmployeeId = testEmployee.Id, // Use FK only
                    Start = new DateTime(2024, 2, 2, 10, 0, 0),
                    End = new DateTime(2024, 2, 2, 16, 0, 0),
                    Task = "Bug fixes and testing"
                }
            };

            modelBuilder.Entity<Employee>().HasData(testEmployee, adminTestEmployee);
            modelBuilder.Entity<Worktime>().HasData(worktimes);
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=app.db"); 
            }
        }
    }
}
