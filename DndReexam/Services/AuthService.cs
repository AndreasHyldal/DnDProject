using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Mvc;

namespace DndReexam.Services
{
public class AuthService
{
    private const string authUriUserId = "http://localhost:5147/api/user-id";
    private readonly HttpClient _http;

    private readonly ILocalStorageService _localStorage;
    

    public AuthService(ILocalStorageService localStorage, HttpClient http)
    {
        _localStorage = localStorage;
        _http = http;
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

    public async Task<string?> GetUserIdAsync()
    {
        Console.WriteLine("Before getting the token!");
        var token = await _localStorage.GetItemAsync<string>("jwt_token");
        Console.WriteLine("Got the token!");
        if (string.IsNullOrEmpty(token))
            return null;

        
        Console.WriteLine("before sec handle");
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(emailClaim))
        {
            // Handle the error appropriately if the claim isn't found
            throw new Exception("Email claim is missing.");
        }

        // var encodedEmail = Uri.EscapeDataString(emailClaim);

        // var requestUri = $"{authUriUserId}?email={encodedEmail}";

        // Console.WriteLine($"emailClaim: {emailClaim}");

        // var userId = await _http.GetFromJsonAsync<string>(requestUri);


        return emailClaim; // Admin or Employee
    }

    
}
}
