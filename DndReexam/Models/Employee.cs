namespace DndReexam.Models
{
    public class Employee
    {
        public int Id { get; set; }
        
        public string FirstName { get; set; } = string.Empty;
        
        public string LastName { get; set; } = string.Empty;
        
        public string Email { get; set; } = string.Empty;
        
        // Although named PasswordHash, for demo purposes this property may hold a plain text value.
        public string PasswordHash { get; set; } = string.Empty;
        
        public string Role { get; set; } = "Employee";
    }
}
