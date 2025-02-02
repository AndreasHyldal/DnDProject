using DndReexam.Models;
using System.Threading.Tasks;

namespace DndReexam.Services
{
    public interface IAuthService
    {
        
        Task<string?> LoginAsync(string EmployeeId, string password);

        Task<bool> RegisterAsync(PersonBaseDTO user);
    }
}
