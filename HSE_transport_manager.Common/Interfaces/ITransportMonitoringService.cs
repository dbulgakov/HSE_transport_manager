using System;
using System.Threading.Tasks;
using HSE_transport_manager.Common.Models;

namespace HSE_transport_manager.Common.Interfaces
{
    public interface ITransportMonitoringService : ITransportService
    {
        Task<DateTime> EstimateTripTimeAsync(Coordinate startingPoint, Coordinate endingPoint);
    }
}
