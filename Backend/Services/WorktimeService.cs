using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

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
    }
}
