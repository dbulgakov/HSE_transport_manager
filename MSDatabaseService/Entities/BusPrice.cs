using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE_transport_manager.Entities
{
    class BusPrice
    {
        public int Id { get; set; }
        [Required]
        public int Price { get; set; }
    }
}
