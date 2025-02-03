using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.Services;
using Backend.Models;
using System.Security.Claims;
using System.Text.Json;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorktimeController(WorktimeService _worktimeService) : ControllerBase
    {
        [Authorize]
        [HttpGet("employee")]
        public async Task<ActionResult<List<Worktime>>> GetMyWorktimes()
        {
            // 1) Get user ID from token
            string userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // string userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            Console.WriteLine(userIdString);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            // 2) Retrieve only that user’s worktimes
            var worktimes = await _worktimeService.GetWorktimesByEmployeeIdAsync(userIdString);
            if (worktimes == null)
            {
                return NotFound(); 
            }

            return Ok(worktimes);
        }

        [Authorize]
        [HttpGet("employee/day/{day}")]
        public async Task<ActionResult<List<Worktime>>> GetMyWorktimes(string day)
        {
            // 1) Get user ID from token
            string userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // string userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            // 2) Retrieve only that user’s worktimes
            var worktimes = await _worktimeService.GetWorktimesByEmployeeIdAndDayAsync(userIdString, day);
            if (worktimes == null)
            {
                return NotFound(); 
            }

            return Ok(worktimes);
        }


        [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult<List<Worktime>>> AddWorktime(Worktime worktime)
        {   
            Console.WriteLine("Isnide of the controller!!!!!!11");

            Console.WriteLine(JsonSerializer.Serialize(worktime, new JsonSerializerOptions { WriteIndented = true }));

            string userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var result = await _worktimeService.PostNewWorktime(worktime, userIdString);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        public class OperationResult<T>
        {
            public bool IsSuccess { get; set; }
            public T? Data { get; set; }
            public string? ErrorMessage { get; set; }
        }

        public class WorktimeRaw
        {
            public DateTime Start { get; set; }

            public DateTime End { get; set; }

            public string Task { get; set; } = string.Empty;
        }



        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<List<Worktime>>> GetByEmployeeId(string employeeEmail)
        {
            var worktimes = await _worktimeService.GetWorktimesByEmployeeIdAsync(employeeEmail);
            return (worktimes == null) ? NotFound() : Ok(worktimes);
        }

        [Authorize]
        [HttpGet("summary")]
        public async Task<ActionResult<List<object>>> GetMyWorktimesByDay()
        {
            // 1) Get user ID from token
            string userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // string userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            Console.WriteLine(userIdString);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }


            // 2) Retrieve only that user’s worktimes
            var summary = await _worktimeService.GetWorktimeSummaryByDayAsync(userIdString);
            if (summary == null)
            {
                return NotFound(); 
            }

            return Ok(summary);
        }


        //  Get Worktime Sum Per Day (For Bar Chart)
        [HttpGet("summary/{employeeId}")]
        public async Task<ActionResult<List<object>>> GetWorktimeSummaryByDay(string employeeEmail)
        {
            var summary = await _worktimeService.GetWorktimeSummaryByDayAsync(employeeEmail);
            return Ok(summary);
        }

        // Add Worktime Entry
        // [HttpPost]
        // public async Task<ActionResult<Worktime>> AddWorktime(Worktime worktime)
        // {
        //     var createdWorktime = await _worktimeService.AddWorktimeAsync(worktime);
        //     return CreatedAtAction(nameof(GetByEmployeeId), new { employeeId = createdWorktime.EmployeeId }, createdWorktime);
        // }

        // Update Worktime Entry
        [HttpPut("{id}")]
        public async Task<ActionResult<Worktime>> UpdateWorktime(int id, Worktime worktime)
        {
            if (id != worktime.Id)
                return BadRequest("Worktime ID mismatch");

            var updatedWorktime = await _worktimeService.UpdateWorktimeAsync(worktime);
            return updatedWorktime is null ? NotFound() : Ok(updatedWorktime);
        }

        // Delete Worktime Entry
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorktime(int id)
        {
            var success = await _worktimeService.DeleteWorktimeAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
