using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GoogleGeoMatrixService.DTO.Response.EstimeTripTimeRequest
{
    class ResponseRow
    {
        [JsonProperty("elements")]
        public List<ResponseElement> ElementsList { get; set; }
    }
}
