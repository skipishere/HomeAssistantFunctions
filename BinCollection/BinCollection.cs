using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace BinCollection
{
    public class BinCollection
    {
#if DEBUG
        private const string Schedule = "0 * * * * *";
#else
        private const string Schedule = "0 0 6,18 * * *";
#endif
        private static HttpClient Client;

        private static void SetupCouncilClient()
        {
            if (Client == null)
            {
                var clientHandler = new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };

                Client = new HttpClient(clientHandler)
                {
                    BaseAddress = new Uri(Environment.GetEnvironmentVariable("CouncilDomain")),
                };
            }
        }

        [FunctionName("BinCollection")]
        public static async Task Run([TimerTrigger(Schedule)]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            SetupCouncilClient();

            await GetData(log);
        }

        private static async Task GetData(ILogger log)
        {
            var date = DateTime.UtcNow;
            var address = string.Format(Environment.GetEnvironmentVariable("CouncilBinUrl"), date.ToString("yyyy-MM-dd"), date.AddDays(15).ToString("yyyy-MM-dd"));
                        
            var response = await Client.GetAsync(address);
            if (response.IsSuccessStatusCode)
            {
                var collections = await response.Content.ReadFromJsonAsync<BinJobs>();
                var binUpdater = new BinUpdater(log);

                await binUpdater.Update(collections);
            }
            else
            {
                log.LogError("Call failed with bad http status code", address, response.StatusCode);
            }
        }
    }
}
