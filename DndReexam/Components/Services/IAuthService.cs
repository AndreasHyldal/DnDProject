using DndReexam.Models;
using System.Threading.Tasks;

namespace DndReexam.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user with their username and password.
        /// Returns the user's role if authentication is successful.
        /// </summary>
        /// <param name="username">The username of the user (EmployeeId).</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>The role of the user if login is successful; otherwise, null.</returns>
        Task<string?> LoginAsync(string username, string password);

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="user">The user to register.</param>
        /// <returns>True if the user was successfully registered; otherwise, false.</returns>
        Task<bool> RegisterAsync(PersonBaseDTO user);
    }
}
