using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YandexScheduleService.DTO.Response.ThreadListResponse
{
    class TrainThread
    {
        [JsonProperty("thread")]
        public TrainInfo TrainInfo { get; set; }

        [JsonProperty("departure")]
        public DateTime? DepartureTime { get; set; }

        [JsonProperty("arrival")]
        public DateTime? ArrivalTime { get; set; }

        [JsonProperty("from")]
        public TrainStation DepartureStation { get; set; }

        [JsonProperty("to")]
        public TrainStation ArrivalStation { get; set; }
    }
}
