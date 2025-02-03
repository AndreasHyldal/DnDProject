using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using Blazored.LocalStorage;
using System.Text.Json;

namespace DndReexam.Services
{
    public class WorktimeService
    {
        private readonly HttpClient _http;

        private const string WorktimeApiUrl = "http://localhost:5147/api/worktime";
        private const string EmployeeApiUrl = "http://localhost:5147/employees";
        private readonly AuthService _authService; // Injected AuthService
        public WorktimeService(HttpClient http, AuthService authService)
        {
            _http = http;
            _authService = authService;
            _http.Timeout = TimeSpan.FromSeconds(30);

        }
        // Get worktime summary for an employee (for chart)
        public async Task<List<WorktimeSummary>> GetWorktimeSummaryAsync(string token)
        {
            // Set the Authorization header with the bearer token
            _http.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _http.GetFromJsonAsync<List<WorktimeSummary>>($"{WorktimeApiUrl}/summary");
                return response ?? new List<WorktimeSummary>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching worktime summary: {ex.Message}");
                return new List<WorktimeSummary>(); // Return empty list on failure
            }
        }

        public async Task<List<WorktimeRaw>> GetWorktimeRawAsync(string token)
        {
            // Set the Authorization header with the bearer token
            _http.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _http.GetFromJsonAsync<List<WorktimeRaw>>(
                    $"{WorktimeApiUrl}/employee"
                );
                return response ?? new List<WorktimeRaw>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching worktime raw data: {ex.Message}");
                return new List<WorktimeRaw>(); 
            }
        }
        
        public async Task<List<WorktimeRaw>> GetWorktimeRawTodayAsync(string token, string day)
        {
            // Set the Authorization header with the bearer token
            _http.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _http.GetFromJsonAsync<List<WorktimeRaw>>(
                    $"{WorktimeApiUrl}/employee/day/{day}"
                );
                return response ?? new List<WorktimeRaw>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching worktime raw data: {ex.Message}");
                return new List<WorktimeRaw>(); 
            }
        }


        // Add a new worktime entry
        public async Task<bool> AddWorktimeAsync(WorktimeRaw worktime, string token)
        {
            Console.WriteLine("1. INSIDE OF THE AddWorktimeAsync!!!");
            // Set the Authorization header with the bearer token
            _http.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
            string? userId = await _authService.GetUserIdAsync();
            Console.WriteLine($"userId: {userId}");
            Worktime wktm = ConvertWorktimeRawToWorktime(worktime, int.Parse(userId));
            
            try 
            {
            Console.WriteLine("INSIDE OF THE AddWorktimeAsync BUT IN TRY CLAUSE!!!");
            Console.WriteLine(JsonSerializer.Serialize(wktm, new JsonSerializerOptions { WriteIndented = true }));
            var response = await _http.PostAsJsonAsync($"{WorktimeApiUrl}/add", wktm);
            Console.WriteLine($"after getting the response {response}");
            
            return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching worktime raw data: {ex.Message}");
                return false;
            }
        }

        // Get all employees
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            try
            {
                var response = await _http.GetFromJsonAsync<List<Employee>>(EmployeeApiUrl);
                return response ?? new List<Employee>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching employees: {ex.Message}");
                return new List<Employee>();
            }
        }

        // Add a new employee
        public async Task<bool> AddEmployeeAsync(Employee employee, string password)
        {
            var payload = new
            {
                employee.FirstName,
                employee.LastName,
                employee.Email,
                employee.Role,
                Password = password
            };

            try
            {
                var response = await _http.PostAsJsonAsync(EmployeeApiUrl, payload);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error adding employee: {ex.Message}");
                return false;
            }
        }

        // Update employee details
        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"{EmployeeApiUrl}/{employee.Id}", employee);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error updating employee: {ex.Message}");
                return false;
            }
        }

        // Delete an employee
        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"{EmployeeApiUrl}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error deleting employee: {ex.Message}");
                return false;
            }
        }

        public Worktime ConvertWorktimeRawToWorktime(WorktimeRaw raw, int employeeId)
        {
            Console.WriteLine($"The emplyeeid: {employeeId}");
            return new Worktime
            {
                EmployeeId = employeeId,
                Start = raw.Start,
                End = raw.End,
                Task = raw.Task
                // Optionally: Leave Employee null, or set it if you have the Employee object.
            };
        }
        // public WorktimeRaw ConvertWorktimeToWorktimeRaw(Worktime worktime)
        // {
        //     return new WorktimeRaw
        //     {
        //         EmployeeId = employeeId,
        //         Start = raw.Start,
        //         End = raw.End,
        //         Task = raw.Task
        //         // Optionally: Leave Employee null, or set it if you have the Employee object.
        //     };
        // }
    }


    public class WorktimeSummary
    {
        [JsonPropertyName("date")]
        public required string Date { get; set; }

        [JsonPropertyName("totalHours")]
        public double TotalHours { get; set; }
    }

    public class WorktimeRaw
    {
        [JsonPropertyName("start")]
        public DateTime Start { get; set; }

        [JsonPropertyName("end")]
        public DateTime End { get; set; }

        [JsonPropertyName("task")]
        public string Task { get; set; } = string.Empty;
    }
}
