using System;
using System.ComponentModel.DataAnnotations;

namespace HSE_transport_manager.Entities
{
    class PublicTransportPrice
    {
        public int Id { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public DateTime ModifiedDate { get; set; }
    }
}
