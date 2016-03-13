using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE_transport_manager.Entities
{
    class LocalTrainStop
    {
        public int Id { get; set; }
        [Required]
        public LocalTrainStation Station { get; set; }
        [Required]
        public DateTime ArrivalTime { get; set; }
        [Required]
        public DateTime ElapsedTime { get; set; }
    }
}
