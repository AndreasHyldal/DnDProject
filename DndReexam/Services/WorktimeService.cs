using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;
using System.Text.Json.Serialization;

namespace DndReexam.Services
{
    public class WorktimeService(HttpClient _http)
    {
        private const string ApiUrl = "http://localhost:5147/api/worktime";

        // Get worktime summary for an employee (for chart)
        public async Task<List<WorktimeSummary>> GetWorktimeSummaryAsync(int employeeId)
        {
                try
                {
                    var response = await _http.GetFromJsonAsync<List<WorktimeSummary>>($"{ApiUrl}/summary/{employeeId}");
                    return response ?? new List<WorktimeSummary>();
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Error fetching worktime summary: {ex.Message}");
                    return new List<WorktimeSummary>(); // Return empty list on failure
                }
        }
        public async Task<List<WorktimeRaw>> GetWorktimeRawAsync(int employeeId)
        {
                try
                {
                    var response = await _http.GetFromJsonAsync<List<WorktimeRaw>>($"{ApiUrl}/employee/{employeeId}");
                    return response ?? new List<WorktimeRaw>();
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Error fetching worktime summary: {ex.Message}");
                    return new List<WorktimeRaw>(); // Return empty list on failure
                }
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
        public string Task { get; set; } = String.Empty;
    }

    
}
