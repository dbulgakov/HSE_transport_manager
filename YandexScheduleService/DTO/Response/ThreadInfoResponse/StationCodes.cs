using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YandexScheduleService.DTO.Response.ThreadInfoResponse
{
    class StationCodes
    {
        [JsonProperty("yandex")]
        public string YandexStationCode { get; set; }
    }
}
