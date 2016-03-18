using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YandexScheduleService.DTO.Response.ThreadInfoResponse
{
    class TrainStation
    {
        [JsonProperty("codes")]
        public StationCodes StationCode { get; set; }

        [JsonProperty("title")]
        public string StationName { get; set; }
    }
}
