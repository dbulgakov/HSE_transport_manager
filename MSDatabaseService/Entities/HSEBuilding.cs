using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSDatabaseService.Entities
{
    class HSEBuilding
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public List<SubwayStation> SubwayStation { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
    }
}
