using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace HSE_transport_manager.Entities
{
    class Dormitory
    {
        public int Id { get; set; }
        [Required]
        public List<string> Name { get; set; }
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
        public bool CheckDubkiBus { get; set; }
        public List<PublicTransport> To { get; set; }
        public List<PublicTransport> From { get; set; }
        
    }
}
