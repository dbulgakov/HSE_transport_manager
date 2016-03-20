using HSE_transport_manager.Entities;
using MSDatabaseService.Entities;
using System.Data.Entity;

namespace MSDatabaseService
{
    class Context: DbContext  
    {
        public DbSet<SubwayStation> SubwayStations { get; set; }
        public DbSet<SubwayElapsedTime> SubwayRouteElapsedTime { get; set; }
        public DbSet<LocalTrainSchedule> LocalTrainsSchedule { get; set; }
        public DbSet<LocalTrainStation> LocalTrainStations { get; set; }
        public DbSet<LocalTrainStop> LocalTrainStops { get; set; }
        public DbSet<LocalTrainPrice> LocalTrainPrices { get; set; }
        public DbSet<HSEBuilding> HSEBuildings { get; set; }
        public DbSet<Dormitory> Dormitories { get; set; }
        public DbSet<DubkiBusSchedule> DubkiBusesSchedule { get; set; }
        public DbSet<PublicTransport> PublicTransportSchedule { get; set; }
        public DbSet<PublicTransportPrice> PublicTransportPrices { get; set; }
        public DbSet<DayofWeek> DayofWeek { get; set; }
        public DbSet<TransportType> TransportTypes { get; set; }
        public Context()
            : base("HSE_transport_manager")
        {

        }
    }
}
