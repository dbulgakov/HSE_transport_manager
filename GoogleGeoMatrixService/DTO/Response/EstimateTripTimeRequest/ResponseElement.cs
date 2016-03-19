using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GoogleGeoMatrixService.DTO.Response.EstimateTripTimeRequest
{
    class ResponseElement
    {
        [JsonProperty("duration")]
        public TripDuration TripDuration { get; set; }
    }
}
