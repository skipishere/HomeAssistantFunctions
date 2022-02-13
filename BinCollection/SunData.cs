using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BinCollection
{
    internal class SunData
    {
        //{"results":{"sunrise":"7:19:19 AM","sunset":"5:11:30 PM","solar_noon":"12:15:25 PM","day_length":"09:52:11","civil_twilight_begin":"6:45:34 AM","civil_twilight_end":"5:45:16 PM","nautical_twilight_begin":"6:05:26 AM","nautical_twilight_end":"6:25:23 PM","astronomical_twilight_begin":"5:25:54 AM","astronomical_twilight_end":"7:04:55 PM"},"status":"OK"}
        [JsonPropertyName("results")]
        public ResultData Result { get; set; }

        internal record struct ResultData
        {
            [JsonPropertyName("day_length")]
            public string DayLength { get; set; }
        }
    }
}
