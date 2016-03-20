using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE_transport_manager.Entities
{
    class SubwayElapsedTime
    {
        public int Id { get; set; }
        [Required]
        public SubwayStation StationFrom { get; set; }
        [Required]
        public SubwayStation StationTo { get; set; }
        [Required]
        public int ElapsedTime { get; set; }
    }
}
