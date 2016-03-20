using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MSDatabaseService.Entities;

namespace HSE_transport_manager.Entities
{
    class LocalTrainSchedule
    {
        [Key]
        public DateTime DepartureTime{ get; set; }
        [Required]
        public string Uid { get; set; }
        [Required]
        public LocalTrainStation DepartureStation { get; set; }
        [Required]
        public LocalTrainStation ArrivalStation { get; set; }
        public List<LocalTrainStop> Stops { get; set; }
        [Required]
        public TransportType Type { get; set; }
    }
}
