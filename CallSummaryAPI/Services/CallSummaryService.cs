using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Text.Json.Serialization;
using CallSummaryAPI.Models;

namespace CallSummaryAPI.Services
{
    public class CallSummaryService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CallSummaryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CallSummary> GetCallSummary(DateTime date)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync("https://raw.githubusercontent.com/intulse/interview/main/interview-test-data.json");
            var callData = JsonConvert.DeserializeObject<List<Call>>(response);

            if (callData == null)
            {
                return new CallSummary
                {
                    SummaryDate = date.ToString("yyyy-MM-dd"),
                    TotalCalls = 0,
                    TotalDurationInSeconds = 0,
                    AvgDurationInSeconds = 0,
                    Hours = Enumerable.Range(8, 10).Select(hour => new HourlySummary
                    {
                        Beginning = $"{hour:00}:00",
                        TotalCalls = 0,
                        TotalDurationInSeconds = 0,
                        AvgDurationInSeconds = 0
                    }).ToList()
                };
            }

            var filteredCalls = callData.Where(c => c.CallDate.Date == date.Date).ToList();

            var totalCalls = filteredCalls.Count;
            var totalDuration = filteredCalls.Sum(c => c.Duration?.Seconds ?? 0);

            var avgDuration = totalCalls > 0 ? totalDuration / totalCalls : 0;

            var hourlySummary = Enumerable.Range(8, 10).Select(hour =>
            {
                var callsInHour = filteredCalls.Where(c => c.CallDate.Hour == hour).ToList();
                var totalDurationInHour = callsInHour.Sum(c => c.Duration?.Seconds ?? 0);
                var totalCallsInHour = callsInHour.Count;
                var AvgDurationInHour = totalCallsInHour > 0 ? totalDurationInHour / totalCallsInHour : 0;

                return new HourlySummary
                {
                    Beginning = $"{hour:00}:00",
                    TotalCalls = totalCallsInHour,
                    TotalDurationInSeconds = totalDurationInHour,
                    AvgDurationInSeconds = AvgDurationInHour
                };
            }).ToList();

            return new CallSummary
            {
                SummaryDate = date.ToString("yyyy-MM-dd"),
                TotalCalls = totalCalls,
                TotalDurationInSeconds = totalDuration,
                AvgDurationInSeconds = avgDuration,
                Hours = hourlySummary
            };
        }
    }
}


public class CallSummary
{
    public string SummaryDate { get; set; } = string.Empty;
    public int TotalCalls { get; set; }
    public int TotalDurationInSeconds { get; set; }
    public double AvgDurationInSeconds { get; set; }
    public List<HourlySummary> Hours { get; set; } = new List<HourlySummary>();
}

public class HourlySummary
{
    public string Beginning { get; set; } = string.Empty;
    public int TotalCalls {  set; get; }
    public int TotalDurationInSeconds { set; get; }
    public double AvgDurationInSeconds { set; get; }
}