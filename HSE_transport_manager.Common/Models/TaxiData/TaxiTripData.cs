using System;
using HSE_transport_manager.Common.Interfaces;

namespace HSE_transport_manager.Common.Models.TaxiData
{


    public class TaxiTripData
    {
        public double Price { get; set; }
        public DateTime Duration { get; set; }
        public Coordinate StartingPoint { get; set; }
        public Coordinate FinishPoint { get; set; }
        public Transport TransportType { get; set; }
    }
}
