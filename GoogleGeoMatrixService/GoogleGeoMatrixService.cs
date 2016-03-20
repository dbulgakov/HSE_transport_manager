using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GoogleGeoMatrixService.DTO.Response.EstimateTripTimeRequest;
using HSE_transport_manager.Common.Interfaces;
using HSE_transport_manager.Common.Models;
using HSE_transport_manager.Common.Models.TaxiData;
using Newtonsoft.Json;

namespace GoogleGeoMatrixService
{
    class GoogleGeoMatrixService : ITransportMonitoringService
    {

        private const string ApiUrl = "https://maps.googleapis.com/maps/api";

        private string _authKey;

        private RequestBuilder _requestBuilder; 

        public void Initialize(string authKey)
        {
            _authKey = authKey;
            _requestBuilder = new RequestBuilder(ApiUrl, _authKey);
        }

        public async Task<int> EstimateTripTimeAsync(Coordinate startingPoint, Coordinate endingPoint)
        {
            return await Task.Run(() =>
            {
                var requestString = _requestBuilder.TimeEstimateRequest(startingPoint, endingPoint);
                var client = new HttpClient();
                var responseString = client.GetAsync(requestString).Result.Content.ReadAsStringAsync().Result;
                var deserializedResponse = JsonConvert.DeserializeObject<EstimateTimeResponse>(responseString);

                return
                    new DateTime().AddSeconds(deserializedResponse.RowList[0].ElementsList[0].TripDuration.DurationTime).Minute;
            });
        }
    }
}
