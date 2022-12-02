using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace BinCollection
{
    public class SlackPost
    {
#if DEBUG
        private const string Schedule = "0 * * * * *";
#else
        private const string Schedule = "0 0 5 * * *";
#endif

        [FunctionName("SlackPost")]
        public static async Task Run([TimerTrigger(Schedule)]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var client = new HttpClient()
            {
                BaseAddress = new Uri(Environment.GetEnvironmentVariable("Slack"))
            };

            var data = new { day = DateTime.UtcNow.Day.ToString() };
            await client.PostAsJsonAsync(Environment.GetEnvironmentVariable("SlackWebHook"), data);
        }
    }
}
