using HomeAssistantHelpers;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using static BinCollection.BinJobs;

namespace BinCollection
{
    internal class BinUpdater : HomeAssistantUpdater
    {
        public BinUpdater(ILogger log) : base(log)
        {
        }

        public async Task Update(Job bin)
        {
            var url = $"/api/states/sensor.bin_{bin.Type.ToString().ToLower()}";

            var data = new
            {
                state = bin.Next.ToString("yyyy-MM-dd"),
                attributes = new
                {
                    friendly_name = $"{bin.Type} bin",
                    icon = bin.Type == Job.BinType.Recycling ? "mdi:recycle" : "mdi:trash-can-outline",
                    next_collection = bin.Next,
                }
            };

            await base.PostUpdate(url, data);
        }
    }
}
