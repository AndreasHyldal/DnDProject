using DndReexam.Models;
using Blazored.LocalStorage;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DndReexam.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        // Handles user login
        public async Task<string?> LoginAsync(string email, string password)
        {
            var loginRequest = new { Email = email, Password = password };
            var content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");

            // Call backend login API
            var response = await _httpClient.PostAsync("/api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent);

                // Save JWT token and user role in local storage
                if (loginResponse != null)
                {
                    await _localStorage.SetItemAsync("authToken", loginResponse.Token);
                    await _localStorage.SetItemAsync("userRole", loginResponse.Role);
                }

                return loginResponse?.Role;
            }

            return null;
        }

        // Handles user registration
        public async Task<bool> RegisterAsync(PersonBaseDTO user)
        {
            var content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");

            // Call backend register API
            var response = await _httpClient.PostAsync("/api/auth/register", content);

            return response.IsSuccessStatusCode;
        }

        // Handles logout
        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("userRole");
        }

        // Provides an HttpClient with JWT Authorization header
        public async Task<HttpClient> GetHttpClientWithAuthAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrEmpty(token))
            {
                if (_httpClient.DefaultRequestHeaders.Authorization == null ||
                    _httpClient.DefaultRequestHeaders.Authorization.Parameter != token)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }

            return _httpClient;
        }
    }
}
