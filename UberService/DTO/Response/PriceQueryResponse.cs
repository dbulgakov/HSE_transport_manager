using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UberService.DTO.Response
{
    class PriceQueryResponse
    {
        [JsonProperty("prices")]
        public List<RideOffer> Trips { get; set; }
    }
}
