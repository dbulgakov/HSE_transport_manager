using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UberService.DTO.Response
{
    class RideOffer
    {
        [JsonProperty("localized_display_name")]
        public string Name { get; set; }

        [JsonProperty("high_estimate")]
        public int High { get; set; }

        [JsonProperty("low_estimate")]
        public int Low { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }
    }
}
