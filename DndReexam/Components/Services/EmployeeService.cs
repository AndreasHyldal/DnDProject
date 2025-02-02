using Backend.Models;


namespace DndReexam.Services
{
    public class EmployeeService
    {
        private readonly HttpClient _httpClient;

        public EmployeeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Employee>>("http://localhost:5147/api/employees");
            return response ?? new List<Employee>();
        }

        public async Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5147/api/employees/{employeeId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddEmployeeAsync(Employee employee, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5147/api/employees", employee);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            var response = await _httpClient.PutAsJsonAsync($"http://localhost:5147/api/employees/{employee.Id}", employee);
            return response.IsSuccessStatusCode;
        }
    }
}
