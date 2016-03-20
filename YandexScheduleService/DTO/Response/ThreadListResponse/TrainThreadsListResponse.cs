using System.Collections.Generic;
using Newtonsoft.Json;

namespace YandexScheduleService.DTO.Response.ThreadListResponse
{
    class TrainThreadsListResponse
    {
        [JsonProperty("threads")]
        public List<TrainThread> TrainThreadList { get; set; }
    }
}
