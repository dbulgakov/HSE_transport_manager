using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSE_transport_manager.Common.Models;
using HSE_transport_manager.Common.Models.TaxiData;

namespace GoogleGeoMatrixService
{
    class RequestBuilder
    {
        private readonly string _apiUrl;
        private readonly string _authKey;

        private const string TimeEstimateRequestString =
            "distancematrix/json?units=imperial&origins={0},{1}&destinations={2},{3}&key={4}";

        public RequestBuilder(string apiUrl, string authKey)
        {
            _apiUrl = apiUrl;
            _authKey = authKey;
        }

        public string TimeEstimateRequest(Coordinate startingPoint, Coordinate endingPoint)
        {
            var param = string.Format(
                TimeEstimateRequestString,
                startingPoint.Latitude,
                startingPoint.Longitude,
                endingPoint.Latitude,
                endingPoint.Longitude,
                _authKey);
            return string.Format("{0}/{1}", _apiUrl, param);
        }
    }
}
