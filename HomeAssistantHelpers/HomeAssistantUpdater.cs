using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace HomeAssistantHelpers
{
    public abstract class HomeAssistantUpdater
    {
        private static HttpClient HomeClient;

        private readonly ILogger _log;

        public HomeAssistantUpdater(ILogger log)
        {
            _log = log;

            if (HomeClient == null)
            {
                HomeClient = new HttpClient()
                {
                    BaseAddress = new Uri(Environment.GetEnvironmentVariable("HomeAssistantUrl")),
                };

                HomeClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Environment.GetEnvironmentVariable("HAToken")}");
            }
        }

        protected async Task PostUpdate(string url, object data)
        {
            var response = await HomeClient.PostAsJsonAsync(url, data);

            if (!response.IsSuccessStatusCode)
            {
                _log.LogError("Unable to update", url, response.StatusCode);
            }
        }
    }
}