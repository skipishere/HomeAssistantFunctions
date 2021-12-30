using System.Linq;
using System.Threading.Tasks;

using HomeAssistantHelpers;
using Microsoft.Extensions.Logging;

namespace BinCollection
{
    internal class BinUpdater : HomeAssistantUpdater
    {
        public BinUpdater(ILogger log) : base(log) { }

        public async Task Update(BinJobs binJobs)
        {
            var url = $"/api/states/sensor.bins";
            
            var nextBin = binJobs.Jobs.OrderBy(c => c.Next).FirstOrDefault();

            var data = new
            {
                state = nextBin.Next.ToString("yyyy-MM-dd"),
                attributes = new
                {
                    friendly_name = "Bin collection",
                    type = $"{nextBin.Type} bin",
                    icon = nextBin.Type == BinJobs.Job.BinType.Recycling ? "mdi:recycle" : "mdi:trash-can-outline",
                    rubbish = binJobs.Jobs.FirstOrDefault(c => c.Type == BinJobs.Job.BinType.Rubbish).Next,
                    recycling = binJobs.Jobs.FirstOrDefault(c => c.Type == BinJobs.Job.BinType.Recycling).Next,
                }
            };

            await base.PostUpdate(url, data);
        }
    }
}
