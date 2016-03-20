using System;
using System.ComponentModel.DataAnnotations;

namespace MSDatabaseService.Entities
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
