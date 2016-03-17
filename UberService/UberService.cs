using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HSE_transport_manager.Common.Interfaces;
using HSE_transport_manager.Common.Models.TaxiData;
using Newtonsoft.Json;
using UberService.DTO.Response;

namespace UberService
{
    class UberService : ITransportService, ITaxiService
    {
        private const string ApiUrl = "https://api.uber.com";
        private const string ApiVer = "v1";

        private string _authKey;
        
        private RequestBuilder _requestBuilder; 


        public void Initialize(string authKey)
        {
            _authKey = authKey;
            _requestBuilder = new RequestBuilder(ApiUrl, ApiVer, _authKey);
        }

        public async Task<TaxiTripData> GetRouteAsync(Coordinate startingPoint, Coordinate endingPoint)
        {
            return await Task<TaxiTripData>.Factory.StartNew(() =>
            {
                var requestString = _requestBuilder.PriceRequest(startingPoint, endingPoint);
                HttpClient client = new HttpClient();
                var responseString = client.GetAsync(requestString).Result.Content.ReadAsStringAsync();
                var deserializedResponse = JsonConvert.DeserializeObject<PriceQueryResponse>(requestString);
                
                var cheapestTrip = deserializedResponse.Trips.First(p => p.Low == deserializedResponse.Trips.Min(a => a.Low));
                TaxiTripData tripData  = new TaxiTripData
                {
                    StartingPoint = startingPoint,
                    FinishPoint = endingPoint,
                    Duration = new DateTime().AddSeconds(cheapestTrip.Duration),
                    Price = cheapestTrip.High,
                    TransportType = Transport.Taxi
                };
                return tripData;
            });
        }
    }
}
