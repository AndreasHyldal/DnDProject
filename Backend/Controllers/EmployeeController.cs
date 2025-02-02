using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Employee employee)
        {
            var userRole = HttpContext.User.FindFirst("role")?.Value;
            if (userRole != "Admin")
            {
                return Forbid(); // Only admins can add employees
            }

            var newEmployee = await _employeeService.AddEmployeeAsync(employee, "DefaultPassword123!");
            return CreatedAtAction(nameof(GetAll), new { id = newEmployee.Id }, newEmployee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Employee employee)
        {
            var userRole = HttpContext.User.FindFirst("role")?.Value;
            if (userRole != "Admin")
            {
                return Forbid(); // Only admins can update employees
            }

            var updatedEmployee = await _employeeService.UpdateEmployeeAsync(employee);
            if (updatedEmployee == null)
            {
                return NotFound();
            }

            return Ok(updatedEmployee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userRole = HttpContext.User.FindFirst("role")?.Value;
            if (userRole != "Admin")
            {
                return Forbid(); // Only admins can delete employees
            }

            var success = await _employeeService.DeleteEmployeeAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
