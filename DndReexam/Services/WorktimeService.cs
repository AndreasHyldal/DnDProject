using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;
using System.Text.Json.Serialization;

namespace DndReexam.Services
{
    public class WorktimeService
    {
        private readonly HttpClient _http;
        private const string WorktimeApiUrl = "http://localhost:5147/api/worktime";
        private const string EmployeeApiUrl = "http://localhost:5147/employees";

        public WorktimeService(HttpClient http)
        {
            _http = http;
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
                    $"{WorktimeApiUrl}/employee/{day}"
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
        public async Task<bool> AddWorktimeAsync(Worktime worktime)
        {
            var response = await _http.PostAsJsonAsync($"{WorktimeApiUrl}", worktime);
            return response.IsSuccessStatusCode;
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
        public required DateTime Start { get; set; }

        [JsonPropertyName("end")]
        public required DateTime End { get; set; }

        [JsonPropertyName("task")]
        public string Task { get; set; } = string.Empty;
    }
}
