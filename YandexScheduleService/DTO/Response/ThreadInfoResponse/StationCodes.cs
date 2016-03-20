using Newtonsoft.Json;

namespace YandexScheduleService.DTO.Response.ThreadInfoResponse
{
    class StationCodes
    {
        [JsonProperty("yandex")]
        public string YandexStationCode { get; set; }
    }
}
