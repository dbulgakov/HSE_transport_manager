using MSDatabaseService.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSDatabaseService.Entities
{
    class PublicTransport
    {
        [Key]
        public int Trip { get; set; }
        [Required]
        public DateTime DepartureTime { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public string From { get; set; }
        [Required]
        public PublicTransportPrice Price { get; set; }
        [Required]
        public string To { get; set; }
        [Required]
        public TransportType Type { get; set; }
        public List<DayofWeek> DayOfWeek { get; set; }
        public List<Dormitory> Dormitory { get; set; }
    }
}
