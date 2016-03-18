using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HSE_transport_manager.Entities
{
    class LocalTrainStation
    { 
        [Key]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
