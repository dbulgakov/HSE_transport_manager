using System.Globalization;
using HSE_transport_manager.Common.Models;

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
                startingPoint.Latitude.ToString().Replace(',','.'),
                startingPoint.Longitude.ToString().Replace(',', '.'),
                endingPoint.Latitude.ToString().Replace(',', '.'),
                endingPoint.Longitude.ToString().Replace(',', '.'),
                _authKey);
            return string.Format("{0}/{1}/{2}", _apiUrl, _apiVer, param);
        }
    }
}
