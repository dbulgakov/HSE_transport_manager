using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE_transport_manager.Entities
{
    class BusSchedule
    {
        [Key]
        public DateTime DepartureTime { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public SubwayStation Destination { get; set; }
        [Required]
        public BusPrice Price { get; set; }
    }
}
