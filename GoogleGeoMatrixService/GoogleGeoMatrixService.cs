using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSE_transport_manager.Common.Interfaces;
using HSE_transport_manager.Common.Models;

namespace GoogleGeoMatrixService
{
    class GoogleGeoMatrixService : ITransportService, ITransportMonitoringService
    {

        private const string ApiUrl = "https://maps.googleapis.com/maps/api";

        private string _authKey;

        private RequestBuilder _requestBuilder; 

        public void Initialize(string authKey)
        {
            _authKey = authKey;
            _requestBuilder = new RequestBuilder(ApiUrl, _authKey);
        }

        public int EstimateTripTime(Coordinate startingPoint, Coordinate endingPoint)
        {
            throw new NotImplementedException();
        }
    }
}
