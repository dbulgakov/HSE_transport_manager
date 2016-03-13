using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HSE_transport_manager.Entities
{
    class LocalTrainSchedule
    {
        [Key]
        public DateTime DepartureTime{ get; set; }
        [Required]
        public string Uid { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public List<LocalTrainStop> Stops { get; set; }
    }
}
