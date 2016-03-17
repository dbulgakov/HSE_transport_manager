using HSE_transport_manager.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Context()
            : base("HSE_transport_manager")
        {

        }
    }
}
