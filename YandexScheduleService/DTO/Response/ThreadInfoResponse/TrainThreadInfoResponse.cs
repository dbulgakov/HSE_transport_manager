﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace YandexScheduleService.DTO.Response.ThreadInfoResponse
{
    class TrainThreadInfoResponse
    {
        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }

        [JsonProperty("stops")]
        public List<TrainStop> TrainStops { get; set; }
    }
}
