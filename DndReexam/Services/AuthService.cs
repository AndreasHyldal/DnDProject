using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;


namespace DndReexam.Services
{
public class AuthService
{
    private readonly ILocalStorageService _localStorage;

    public AuthService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _localStorage.GetItemAsync<string>("jwt_token");
    }

    public async Task<string?> GetUserRoleAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("jwt_token");

        if (string.IsNullOrEmpty(token))
            return null;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

        return roleClaim?.Value; // Admin or Employee
    }
}
}
