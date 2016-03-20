using System.ComponentModel.DataAnnotations;

namespace HSE_transport_manager.Entities
{
    class SubwayElapsedTime
    {
        public int Id { get; set; }
        [Required]
        public SubwayStation StationFrom { get; set; }
        [Required]
        public SubwayStation StationTo { get; set; }
        [Required]
        public int ElapsedTime { get; set; }
    }
}
