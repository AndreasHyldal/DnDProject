using DndReexam.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DndReexam.Services
{
    public class AuthService : IAuthService
    {
        // In-memory user store
        private readonly List<PersonBaseDTO> _users = new()
        {
            new PersonBaseDTO { EmployeeId = "admin", Password = "admin123", Role = "Admin" },
            new PersonBaseDTO { EmployeeId = "user", Password = "user123", Role = "User" }
        };

        // Handle user login
        public Task<string?> LoginAsync(string username, string password)
        {
            var user = _users.FirstOrDefault(u => u.EmployeeId == username && u.Password == password);
            return Task.FromResult(user?.Role);
        }

        public Task<bool> RegisterAsync(PersonBaseDTO user)
        {
            if (_users.Any(u => u.EmployeeId == user.EmployeeId))
            {
                return Task.FromResult(false);
            }

        _users.Add(user);
        return Task.FromResult(true);
        }

    }
}
