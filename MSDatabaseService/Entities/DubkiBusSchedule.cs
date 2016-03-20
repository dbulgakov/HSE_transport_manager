using MSDatabaseService.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HSE_transport_manager.Entities
{
    class DubkiBusSchedule
    {
        [Key]
        public int Trip { get; set; }
        [Required]
        public DateTime DepartureTime { get; set; }
        [Required]
        public string From { get; set; }
        [Required]
        public List<DayofWeek> DayOfWeek { get; set; }
        [Required]
        public string To { get; set; }
        [Required]
        public TransportType Type { get; set; }
    }
}
