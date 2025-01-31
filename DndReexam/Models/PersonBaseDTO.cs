namespace DndReexam.Models
{
    public class PersonBaseDTO
    {
        public string EmployeeId { get; set; } = string.Empty; // Required for login
        public string Password { get; set; } = string.Empty; // Required for login
        public string Role { get; set; } = "User"; // Default role is "User"
    }
}
