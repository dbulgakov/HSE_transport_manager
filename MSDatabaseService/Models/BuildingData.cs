using System.Collections.Generic;

namespace MSDatabaseService.Models
{
    public class BuildingData
    {
            public string Name { get; set; }
            public string Region { get; set; }
            public string City { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string Address { get; set; }
            public List<string> SubwayStation { get; set; }
            public string LocalTrainStation { get; set; }
            public bool CheckDubkiBus { get; set; }
            public List<string> PublicTransport { get; set; }
            public string TramStop { get; set; }
    }
}
