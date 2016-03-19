using System.Threading.Tasks;
using HSE_transport_manager.Common.Models;
using HSE_transport_manager.Common.Models.TaxiData;

namespace HSE_transport_manager.Common.Interfaces
{
    public interface ITaxiService : ITransportService

    {
        Task<TaxiTripData> GetRouteAsync(Coordinate startingPoint, Coordinate endingPoint);
    }
}
