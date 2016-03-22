using System.ComponentModel.DataAnnotations;

namespace MSDatabaseService.Entities
{
    class LocalTrainStation
    { 
        [Key]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
