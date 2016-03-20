using HSE_transport_manager.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSDatabaseService.Entities
{
    class DayofWeek
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<DubkiBusSchedule> DubkiBusSchedule { get; set; }
        public List<PublicTransport> PublicTransport { get; set; }
    }
}
