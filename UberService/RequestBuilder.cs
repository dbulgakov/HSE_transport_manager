using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSE_transport_manager.Common.Models;
using HSE_transport_manager.Common.Models.TaxiData;

namespace UberService
{
    class RequestBuilder
    {
        private readonly string _apiUrl;
        private readonly string _apiVer;
        private readonly string _authKey;

        private const string PriceRequestString =
            "estimates/price?start_latitude={0}&start_longitude={1}&end_latitude={2}&end_longitude={3}&server_token={4}";


        public RequestBuilder(string apiUrl, string apiVer, string authKey)
        {
            _apiUrl = apiUrl;
            _apiVer = apiVer;
            _authKey = authKey;
        }

        public string PriceRequest(Coordinate startingPoint, Coordinate endingPoint)
        {
            var param = string.Format(
                PriceRequestString,
                startingPoint.Latitude.ToString("R"),   
                startingPoint.Longitude.ToString("R"),
                endingPoint.Latitude.ToString("R"),
                endingPoint.Longitude.ToString("R"),
                _authKey);
            return string.Format("{0}/{1}/{2}", _apiUrl, _apiVer, param);
        }
    }
}
