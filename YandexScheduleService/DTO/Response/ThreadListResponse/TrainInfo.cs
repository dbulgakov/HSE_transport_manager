using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YandexScheduleService.DTO.Response.ThreadListResponse
{
    class TrainInfo
    {
        [JsonProperty("uid")]
        public string TrainUid { get; set; }

        [JsonProperty("express_type")]
        public string TrainExpressType { get; set; }
    }
}
