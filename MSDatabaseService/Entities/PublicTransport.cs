using MSDatabaseService.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE_transport_manager.Entities
{
    class PublicTransport
    {
        [Key]
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
    }
}
