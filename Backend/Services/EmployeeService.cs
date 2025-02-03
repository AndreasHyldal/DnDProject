using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Backend.Models;
using Backend.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Services
{
    public class EmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "http://localhost:5147/employees"; // Backend API URL

        public EmployeeService(ApplicationDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        // Get all employees (For Admin use only)
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        // Get employee by ID
        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        // Get employee by email (For login)
        public async Task<Employee?> GetEmployeeByEmailAsync(string email)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
        }

        // Add new employee (For Admins)
        public async Task<Employee> AddEmployeeAsync(Employee employee, string password)
        {
            employee.PasswordHash = HashPassword(password);
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        // Update employee details
        public async Task<Employee?> UpdateEmployeeAsync(Employee employee)
        {
            var existingEmployee = await _context.Employees.FindAsync(employee.Id);
            if (existingEmployee == null) return null;

            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.Email = employee.Email;
            existingEmployee.Role = employee.Role;
            existingEmployee.DateOfBirth = employee.DateOfBirth;
            existingEmployee.HireDate = employee.HireDate;

            await _context.SaveChangesAsync();
            return existingEmployee;
        }

        // Delete employee
        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        // Authenticate employee (For login)
        public async Task<Employee?> AuthenticateEmployeeAsync(string email, string password)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
            if (employee == null || !VerifyPassword(password, employee.PasswordHash))
                return null;

            return employee;
        }

        // Hash password
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        // Verify password
        private bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }

        // Frontend API Calls (Blazor Web Requests)
        public async Task<List<Employee>> FetchAllEmployeesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Employee>>($"{ApiUrl}");
            return response ?? new List<Employee>();
        }

        public async Task<int> GetUserIdFromEmail(string email)
        {
            var user = await _context.Employees
            .Where(u => u.Email == email)
            .Select(u => u.Id) // Only fetch the ID for efficiency
            .FirstOrDefaultAsync();

        return user;
        }

        public async Task<bool> DeleteEmployeeFromApiAsync(int employeeId)
        {
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{employeeId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddEmployeeToApiAsync(Employee employee)
        {
            var response = await _httpClient.PostAsJsonAsync($"{ApiUrl}", employee);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateEmployeeInApiAsync(Employee employee)
        {
            var response = await _httpClient.PutAsJsonAsync($"{ApiUrl}/{employee.Id}", employee);
            return response.IsSuccessStatusCode;
        }
    }
}
