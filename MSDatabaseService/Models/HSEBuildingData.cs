using System.Collections.Generic;

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
