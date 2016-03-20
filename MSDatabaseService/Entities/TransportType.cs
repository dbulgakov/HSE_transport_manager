using System.ComponentModel.DataAnnotations;

namespace MSDatabaseService.Entities
{
    class TransportType
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
