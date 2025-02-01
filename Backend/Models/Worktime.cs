using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Worktime
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        
        [Required]
        public required Employee Employee { get; set; }  // Navigation property

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        [Required]
        [MaxLength(500)] // Adjust the length as needed
        public string Task { get; set; } = String.Empty;
    }
}
