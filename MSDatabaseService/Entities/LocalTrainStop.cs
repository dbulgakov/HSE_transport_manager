using System;
using System.ComponentModel.DataAnnotations;

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
