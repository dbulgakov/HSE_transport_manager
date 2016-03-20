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
