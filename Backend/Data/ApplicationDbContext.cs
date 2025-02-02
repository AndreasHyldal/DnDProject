using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Data;
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

        modelBuilder.Entity<Employee>().HasData(
            new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PasswordHash = HashPassword("Test123!"),
                Role = "Employee",
                DateOfBirth = new DateTime(1990, 5, 15),
                HireDate = new DateTime(2024, 2, 1)
            },
            new Employee
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                PasswordHash = HashPassword("Test123!"),
                Role = "Admin",
                DateOfBirth = new DateTime(1985, 10, 20),
                HireDate = new DateTime(2023, 5, 5)
            }
        );

        // ✅ Seed Worktime Entries (Only EmployeeId, No Navigation Property)
        var worktimes = new List<Worktime>
        {
            new Worktime
            {
                Id = 1,
                EmployeeId = 1, // ✅ Only use FK, don't set Employee
                Start = new DateTime(2024, 2, 1, 9, 0, 0),
                End = new DateTime(2024, 2, 1, 17, 0, 0),
                Task = "Worked on frontend UI"
            },
            new Worktime
            {
                Id = 2,
                EmployeeId = 1, // ✅ Only use FK, don't set Employee
                Start = new DateTime(2024, 2, 2, 10, 0, 0),
                End = new DateTime(2024, 2, 2, 16, 0, 0),
                Task = "Bug fixes and testing"
            },
            new Worktime
            {
                Id = 3,
                EmployeeId = 1, // ✅ Only use FK, don't set Employee
                Start = new DateTime(2024, 2, 3, 10, 0, 0),
                End = new DateTime(2024, 2, 3, 20, 0, 0),
                Task = "Bug fixes and testing"
            },
            new Worktime
            {
                Id = 4,
                EmployeeId = 1, // ✅ Only use FK, don't set Employee
                Start = new DateTime(2024, 2, 4, 10, 0, 0),
                End = new DateTime(2024, 2, 4, 14, 0, 0),
                Task = "Bug fixes and testing"
            },
            new Worktime
            {
                Id = 5,
                EmployeeId = 1, // ✅ Only use FK, don't set Employee
                Start = new DateTime(2024, 2, 5, 10, 0, 0),
                End = new DateTime(2024, 2, 5, 14, 0, 0),
                Task = "Bug fixes and testing"
            },
            new Worktime
            {
                Id = 6,
                EmployeeId = 1, // ✅ Only use FK, don't set Employee
                Start = new DateTime(2024, 2, 6, 10, 0, 0),
                End = new DateTime(2024, 2, 6, 16, 0, 0),
                Task = "Bug fixes and testing"
            },
            new Worktime
            {
                Id = 7,
                EmployeeId = 1, // ✅ Only use FK, don't set Employee
                Start = new DateTime(2024, 2, 7, 10, 0, 0),
                End = new DateTime(2024, 2, 7, 16, 0, 0),
                Task = "Bug fixes and testing"
            },
            new Worktime
            {
                Id = 8,
                EmployeeId = 2, // ✅ Only use FK, don't set Employee
                Start = new DateTime(2024, 2, 1, 10, 0, 0),
                End = new DateTime(2024, 2, 1, 14, 0, 0),
                Task = "Bug fixes and testing"
            },
            new Worktime
            {
                Id = 9,
                EmployeeId = 2, // ✅ Only use FK, don't set Employee
                Start = new DateTime(2024, 2, 2, 10, 0, 0),
                End = new DateTime(2024, 2, 2, 22, 0, 0),
                Task = "Bug fixes and testing"
            },
            new Worktime
            {
                Id = 10,
                EmployeeId = 2, // ✅ Only use FK, don't set Employee
                Start = new DateTime(2024, 2, 3, 10, 0, 0),
                End = new DateTime(2024, 2, 3, 16, 0, 0),
                Task = "Bug fixes and testing"
            }
        };

        modelBuilder.Entity<Worktime>().HasData(worktimes); // ✅ No Employee object here
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
