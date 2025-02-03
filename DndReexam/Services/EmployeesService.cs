using System.IdentityModel.Tokens.Jwt;
using Blazored.LocalStorage;
using Backend.Models;
using System.Net.Http.Headers;
using DndReexam.Models;

namespace DndReexam.Services
{
public class EmployeesService
{
    private readonly HttpClient _http;

    private readonly ILocalStorageService _localStorage;
    private string EmployeesBaseUrl = "http://localhost:5147/api";

    public EmployeesService(ILocalStorageService localStorage, HttpClient http)
    {
        _localStorage = localStorage;
        _http = http;
    }

    public async Task<List<EditableEmployee>> LoadEmployees(string token)
    {
        // Set the Authorization header with the bearer token
        _http.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);

        try
        {
            Console.WriteLine("Inside of LoadEmployees");
            var response = await _http.GetFromJsonAsync<List<EditableEmployee>>(
                $"{EmployeesBaseUrl}/employees"
            );
            return response ?? new List<EditableEmployee>();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error fetching data: {ex.Message}");
            return new List<EditableEmployee>(); 
        }
    }

    //     public class Employee
    // {
    //     public int Id { get; set; }
        
    //     public string FirstName { get; set; } = string.Empty;
        
    //     public string LastName { get; set; } = string.Empty;
        
    //     public string Email { get; set; } = string.Empty;
        
    //     // Although named PasswordHash, for demo purposes this property may hold a plain text value.
    //     public string PasswordHash { get; set; } = string.Empty;
        
    //     public string Role { get; set; } = "Employee";
    // }
}
}