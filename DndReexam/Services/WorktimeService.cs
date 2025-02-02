using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;

namespace DndReexam.Services
{
    public class WorktimeService(HttpClient _http)
    {
        private const string ApiUrl = "http://localhost:5075/api/worktime";

        // Get worktime summary for an employee (for chart)
        public async Task<List<WorktimeSummary>> GetWorktimeSummaryAsync(int employeeId)
        {
            return await _http.GetFromJsonAsync<List<WorktimeSummary>>($"{ApiUrl}/summary/{employeeId}") ?? new List<WorktimeSummary>();
        }

        // Add a new worktime entry
        public async Task<bool> AddWorktimeAsync(Worktime worktime)
        {
            var response = await _http.PostAsJsonAsync($"{ApiUrl}", worktime);
            return response.IsSuccessStatusCode;
        }
    }

    public class WorktimeSummary
    {
        public required string Date { get; set; }
        public double TotalHours { get; set; }
    }
}
