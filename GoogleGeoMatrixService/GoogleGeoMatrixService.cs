using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSE_transport_manager.Common.Interfaces;

namespace GoogleGeoMatrixService
{
    class GoogleGeoMatrixService : ITransportService, ITransportMonitoringService
    {

        public void Initialize(string authKey)
        {
            throw new NotImplementedException();
        }

        public int EstimateTripTime(HSE_transport_manager.Common.Models.Coordinate startingPoint, HSE_transport_manager.Common.Models.Coordinate endingPoint)
        {
            throw new NotImplementedException();
        }
    }
}
