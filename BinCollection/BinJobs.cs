using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BinCollection
{
    internal class BinJobs
    {
        [JsonPropertyName("jobs_FeatureScheduleDates")]
        public IEnumerable<Job> Jobs { get; set; }

        internal record struct Job
        {
            internal enum BinType
            {
                Unknown,
                Rubbish,
                Recycling,
            }

            [JsonPropertyName("jobName")]
            public string Name { get; set; }

            [JsonPropertyName("nextDate")]
            public DateTime Next { get; set; }

            public BinType Type
            {
                get
                {
                    if (Name.Contains("black", StringComparison.OrdinalIgnoreCase))
                    {
                        return BinType.Rubbish;
                    }
                    else if (Name.Contains("green", StringComparison.OrdinalIgnoreCase))
                    {
                        return BinType.Recycling;
                    }

                    return BinType.Unknown;
                }
            }
        }
    }
}
