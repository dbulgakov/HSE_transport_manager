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
