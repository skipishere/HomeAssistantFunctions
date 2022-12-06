using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
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
        private const string Schedule = "0 0 5 1-25 12 *";
#endif
        
        [FunctionName("SlackPost")]
        public static async Task Run([TimerTrigger(Schedule)]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            using var client = new HttpClient()
            {
                BaseAddress = new Uri(Environment.GetEnvironmentVariable("Slack"))
            };

            var puzzleTitle = await PuzzleTitle();
            var data = new { day = DateTime.UtcNow.Day.ToString(), title = puzzleTitle };
            await client.PostAsJsonAsync(Environment.GetEnvironmentVariable("SlackWebHook"), data);
        }

        private static async Task<string> PuzzleTitle()
        {
            try
            {
                using var client = new HttpClient()
                {
                    BaseAddress = new Uri("https://adventofcode.com/"),
                };

                var userAgent = new ProductInfoHeaderValue("(+github.com/skipishere/HomeAssistantFunctions/blob/main/BinCollection/SlackPost.cs by me@stephenj.co.uk)");
                client.DefaultRequestHeaders.UserAgent.Add(userAgent);

                var date = DateTime.UtcNow.Date;
                var address = $"{date.Year}/day/{date.Day}";

                var response = await client.GetAsync(address);
                var regex = new Regex("<article class=\"day-desc\"><h2>--- Day \\d+: (?<title>[\\w -]+) ---<\\/h2>");
                var match = regex.Match(await response.Content.ReadAsStringAsync());
                
                return $" - {match.Groups["title"].Value}";
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
