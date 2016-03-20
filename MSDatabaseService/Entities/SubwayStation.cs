using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MSDatabaseService.Entities;

namespace HSE_transport_manager.Entities
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
        public List<Dormitory> Dormitory { get; set; }
        public List<HSEBuilding> HSEBuilding { get; set; }
        [Required]
        public TransportType Type { get; set; }
    }
}
