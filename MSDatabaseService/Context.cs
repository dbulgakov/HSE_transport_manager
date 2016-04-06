using MSDatabaseService.Entities;
using System.Data.Entity;

namespace MSDatabaseService
{
    class Context: DbContext  
    {
        public DbSet<SubwayStation> SubwayStations { get; set; }
        public DbSet<LocalTrainSchedule> LocalTrainsSchedule { get; set; }
        public DbSet<LocalTrainStation> LocalTrainStations { get; set; }
        public DbSet<LocalTrainStop> LocalTrainStops { get; set; }
        public DbSet<LocalTrainPrice> LocalTrainPrices { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<DubkiBusSchedule> DubkiBusesSchedule { get; set; }
        public DbSet<PublicTransport> PublicTransportSchedule { get; set; }
        public DbSet<PublicTransportPrice> PublicTransportPrices { get; set; }
        public DbSet<DayofWeek> DayofWeek { get; set; }
        public DbSet<TransportType> TransportTypes { get; set; }
        public DbSet<TramStop> TramStop { get; set; }
        public Context()
            : base("HSE_transport_manager")
        {

        }
    }
}
