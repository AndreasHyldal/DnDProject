using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using System.Globalization;

namespace Backend.Services
{
    public class WorktimeService
    {
        private readonly ApplicationDbContext _context;

        public WorktimeService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all worktime entries
        public async Task<List<Worktime>> GetAllWorktimesAsync()
        {
            return await _context.Worktimes.Include(w => w.Employee).ToListAsync();
        }

        // Get worktime entry by ID
        public async Task<Worktime?> GetWorktimeByIdAsync(int id)
        {
            return await _context.Worktimes.Include(w => w.Employee)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        // Add a new worktime entry
        public async Task<Worktime> AddWorktimeAsync(Worktime worktime)
        {
            _context.Worktimes.Add(worktime);
            await _context.SaveChangesAsync();
            return worktime;
        }

        // Update an existing worktime entry
        public async Task<Worktime?> UpdateWorktimeAsync(Worktime worktime)
        {
            var existingWorktime = await _context.Worktimes.FindAsync(worktime.Id);
            if (existingWorktime == null)
            {
                return null;
            }

            existingWorktime.EmployeeId = worktime.EmployeeId;
            existingWorktime.Start = worktime.Start;
            existingWorktime.End = worktime.End;
            existingWorktime.Task = worktime.Task;

            await _context.SaveChangesAsync();
            return existingWorktime;
        }

        // Delete a worktime entry
        public async Task<bool> DeleteWorktimeAsync(int id)
        {
            var worktime = await _context.Worktimes.FindAsync(id);
            if (worktime == null)
            {
                return false;
            }

            _context.Worktimes.Remove(worktime);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Worktime>> GetWorktimesByEmployeeIdAndDayAsync(string employeeEmail, string day)
        {
            // Parse the input string into a DateTime
            DateTime parsedDate = DateTime.ParseExact(day, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime nextDay = parsedDate.AddDays(1);
            // Use Console.WriteLine for proper formatting
            Console.WriteLine($"Parsed Date: {parsedDate:yyyy-MM-dd}");
            Console.WriteLine($"Next Day: {nextDay:yyyy-MM-dd}");

            return await _context.Worktimes
                .Where(w => w.Employee.Email == employeeEmail 
                        && w.Start >= parsedDate 
                        && w.Start < nextDay)
                .OrderBy(w => w.Start) // Order by start time
                .ToListAsync();
        }

        // Get Worktimes for a Specific Employee
        public async Task<List<Worktime>> GetWorktimesByEmployeeIdAsync(string employeeEmail)
        {
            return await _context.Worktimes
                .Where(w => w.Employee.Email == employeeEmail)
                .OrderBy(w => w.Start) // Order by start time
                .ToListAsync();
        }

        // Get Worktime Sum Per Day (For Bar Chart)
        public async Task<List<object>> GetWorktimeSummaryByDayAsync(string employeeEmail)
        {
            var worktimes = await _context.Worktimes
                .Where(w => w.Employee.Email == employeeEmail)
                    .ToListAsync(); // ✅ Fetch data first (Executes SQL)

            var summary = worktimes
                .GroupBy(w => w.Start.Date) // ✅ Perform grouping in memory
                .Select(g => new
                {
                    Date = g.Key,
                    TotalHours = g.Sum(w => (w.End - w.Start).TotalHours) // ✅ Now it works!
                })
                .OrderBy(g => g.Date)
                .ToList();

            return summary.Cast<object>().ToList(); // ✅ Ensure correct return type
        }


        public async Task<OperationResult<List<Worktime>>> PostNewWorktime(Worktime worktime, string userEmail)
        {
            try
            {
                // Retrieve the employee record using the provided email
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == userEmail);
                if (employee == null)
                {
                    return new OperationResult<List<Worktime>>
                    {
                        IsSuccess = false,
                        ErrorMessage = "Employee not found."
                    };
                }

                // Optionally perform additional validation on the worktime here

                // Add the new worktime to the context and save changes
                _context.Worktimes.Add(worktime);
                await _context.SaveChangesAsync();

                // Retrieve the updated list of worktimes for this employee
                var worktimes = await _context.Worktimes
                    .Where(w => w.EmployeeId == employee.Id)
                    .OrderBy(w => w.Start)
                    .ToListAsync();

                return new OperationResult<List<Worktime>>
                {
                    IsSuccess = true,
                    Data = worktimes
                };
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return new OperationResult<List<Worktime>>
                {
                    IsSuccess = false,
                    ErrorMessage = $"An error occurred while posting the new worktime: {ex.Message}"
                };
            }
        }


        public class WorktimeRaw
        {
            public DateTime Start { get; set; }

            public DateTime End { get; set; }

            public string Task { get; set; } = string.Empty;
        }

        public class OperationResult<T>
        {
            public bool IsSuccess { get; set; }
            public T? Data { get; set; }
            public string? ErrorMessage { get; set; }
        }
    }
}
