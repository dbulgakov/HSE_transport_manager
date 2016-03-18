using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YandexScheduleService.DTO.Response.ThreadInfoResponse
{
    class TrainStop
    {
        [JsonProperty("arrival")]
        public DateTime? ArrivalTime { get; set; }

        [JsonProperty("departure")]
        public DateTime? DepartureTime { get; set; }

        [JsonProperty("station")]
        public TrainStation StopStation { get; set; }

        [JsonProperty("duration")]
        public double? TripDuration { get; set; }

        [JsonProperty("stop_time")]
        public double? StopTime { get; set; }
    }
}
