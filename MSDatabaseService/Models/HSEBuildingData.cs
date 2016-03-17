using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSDatabaseService.Models
{
    class HSEBuildingData
    {
        public string[] Name { get; set; }
        public string Address { get; set; }
        public decimal Latitude { get; set; }   
        public decimal Longitude { get; set; }
        public string[] SubwayStation { get; set; }
    }
}
