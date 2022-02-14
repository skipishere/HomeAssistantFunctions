using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace Helpers
{
    public abstract class HomeAssistantUpdater
    {
        private static HttpClient HomeClient = default!;

        private readonly ILogger _log;

        public HomeAssistantUpdater(ILogger log)
        {
            _log = log;

            if (HomeClient == null)
            {
                HomeClient = new HttpClient()
                {
                    BaseAddress = new Uri(Environment.GetEnvironmentVariable("HomeAssistantUrl")!),
                };

                HomeClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Environment.GetEnvironmentVariable("HAToken")}");
            }
        }

        protected async Task PostUpdate(string url, object data)
        {
            var response = await HomeClient.PostAsJsonAsync(url, data);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Unable to update");
            }
        }
    }
}