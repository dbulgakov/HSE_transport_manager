using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSE_transport_manager.Common.Models.TaxiData
{
    public enum Transport
    {
        Suburban,
        Tram,
        Bus,
        Taxi
    }

    public class TaxiTripData
    {
        public double Price { get; set; }
        public DateTime Duration { get; set; }
        public string StartingPoint { get; set; }
        public string FinishPoint { get; set; }
        public Transport TransportType { get; set; }
    }
}
