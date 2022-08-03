using System;
using System.Threading.Tasks;

using Helpers;
using Microsoft.Extensions.Logging;

namespace BinCollection
{
    internal class CovidUpdater : HomeAssistantUpdater
    {
        public CovidUpdater(ILogger log) : base(log) { }

        public async Task Update(string key, object value)
        {
            var url = $"/api/states/sensor.covid_{key.ToLower().Replace(' ', '_')}";

            var data = new
            {
                state = value,
                attributes = new
                {
                    friendly_name = $"{key} cases",
                    unit_of_measurement = "people",
                    icon = "mdi:virus-outline"
                }
            };

            await PostUpdate(url, data);
        }

        public async Task UpdateVaccine(string key, float? value)
        {
            if (!value.HasValue)
            {
                return;
            }

            var url = $"/api/states/sensor.covid_{key.ToLower().Replace(' ', '_')}";

            var data = new
            {
                state = value,
                attributes = new
                {
                    friendly_name = $"{key} dose",
                    unit_of_measurement = "%",
                    icon = "mdi:needle"
                }
            };

            await PostUpdate(url, data);
        }

        public async Task UpdateLastRun()
        {
            var url = $"/api/states/sensor.covid_last_ran";

            var data = new
            {
                state = DateTime.UtcNow,
                attributes = new
                {
                    friendly_name = $"Last ran",
                    unit_of_measurement = "time",
                    icon = "mdi:virus-outline"
                }
            };

            await PostUpdate(url, data);
        }
    }
}
