using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MSDatabaseService.Entities;

namespace MSDatabaseService.Entities
{
    class SubwayStation
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        public List<Building> Building { get; set; }
        [Required]
        public TransportType Type { get; set; }
        public List<PublicTransport> PublicTransport { get; set; }

    }
}
