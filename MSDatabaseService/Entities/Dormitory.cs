using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSDatabaseService.Entities
{
    class Dormitory
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Region { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }
        public List<SubwayStation> SubwayStation { get; set; }
        public LocalTrainStation LocalTrainStation { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public bool CheckDubkiBus { get; set; }
        public List<PublicTransport> PublicTransport{ get; set; }
        
    }
}
