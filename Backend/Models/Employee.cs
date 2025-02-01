using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Backend.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty; // Store hashed password, not plain text

        [Required]
        public string Role { get; set; } = "Employee"; // e.g., "Employee", "Manager", etc.

        public DateTime DateOfBirth { get; set; }

        public DateTime HireDate { get; set; } = DateTime.UtcNow; // Default to now

        public List<Worktime>? Worktimes { get; set; } // Navigation property
    }
}
