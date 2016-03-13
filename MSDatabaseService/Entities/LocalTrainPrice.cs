using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HSE_transport_manager.Entities
{
    class LocalTrainPrice
    {
        public int Id { get; set; }
        [Required]
        public LocalTrainStop Station { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
