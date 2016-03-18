using HSE_transport_manager.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSDatabaseService.Entities
{
    class DayofWeek
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<DubkiBusSchedule> DubkiBusSchedule { get; set; }
    }
}
