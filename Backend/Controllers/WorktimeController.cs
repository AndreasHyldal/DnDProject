using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.Services;
using Backend.Models;
using System.Security.Claims;

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
            string userIdString = User.FindFirst("sub")?.Value 
                                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine(userIdString);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            if (!int.TryParse(userIdString, out int userId))
            {
                return Unauthorized();
            }

            // 2) Retrieve only that user’s worktimes
            var worktimes = await _worktimeService.GetWorktimesByEmployeeIdAsync(userId);
            if (worktimes == null)
            {
                return NotFound(); 
            }

            return Ok(worktimes);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<List<Worktime>>> GetByEmployeeId(int employeeId)
        {
            var worktimes = await _worktimeService.GetWorktimesByEmployeeIdAsync(employeeId);
            return (worktimes == null) ? NotFound() : Ok(worktimes);
        }

        //  Get Worktime Sum Per Day (For Bar Chart)
        [HttpGet("summary/{employeeId}")]
        public async Task<ActionResult<List<object>>> GetWorktimeSummaryByDay(int employeeId)
        {
            var summary = await _worktimeService.GetWorktimeSummaryByDayAsync(employeeId);
            return Ok(summary);
        }

        // Add Worktime Entry
        [HttpPost]
        public async Task<ActionResult<Worktime>> AddWorktime(Worktime worktime)
        {
            var createdWorktime = await _worktimeService.AddWorktimeAsync(worktime);
            return CreatedAtAction(nameof(GetByEmployeeId), new { employeeId = createdWorktime.EmployeeId }, createdWorktime);
        }

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
