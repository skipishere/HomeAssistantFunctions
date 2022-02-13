using System.Linq;
using System.Threading.Tasks;

using Helpers;
using Microsoft.Extensions.Logging;

namespace BinCollection
{
    internal class DayLightUpdater : HomeAssistantUpdater
    {
        public DayLightUpdater(ILogger log) : base(log) { }

        public async Task Update(SunData sunData)
        {
            var url = $"/api/states/sensor.daylight";
            
            var data = new
            {
                state = sunData.Result.DayLength,
                attributes = new
                {
                    //device_class = "date",
                    friendly_name = "Amount of daylight",
                    icon = "mdi:weather-sunset-up",
                }
            };

            await base.PostUpdate(url, data);
        }
    }
}
