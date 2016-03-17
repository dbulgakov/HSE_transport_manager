using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSDatabaseService.Models
{
    public class HSEBuildingData
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }   
        public double Longitude { get; set; }
        public List<string> SubwayStation { get; set; }
    }
}
