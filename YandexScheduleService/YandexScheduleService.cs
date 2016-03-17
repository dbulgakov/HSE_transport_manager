using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSE_transport_manager.Common.Interfaces;
using HSE_transport_manager.Common.Models.TrainSchedulesData;
using Newtonsoft.Json;
using System.Net.Http;
using YandexScheduleService.DTO.Response.ThreadInfoResponse;

namespace YandexScheduleService
{
    class YandexScheduleService: ITransportService, ITransportSchedulerService
    {
        private const string ApiUrl = "http://api.rasp.yandex.net";
        private const string ApiVer = "v1.0";

        private string _authKey;

        private RequestBuilder _requestBuilder; 

        public void Initialize(string authKey)
        {
            _authKey = authKey;
            _requestBuilder = new RequestBuilder(ApiUrl, ApiVer, _authKey);
        }

        public async Task<DailyTrainSchedule> GetDailyScheduleAsync(string startingStationCode, string endingStationCode)
        {
            throw new NotImplementedException();
        }

        public async Task<SingleTrainSchedule> GetScheduleAsync(string transportId)
        {
            throw new NotImplementedException();
        }


        public Task<SingleTrainSchedule> GetScheduleAsync(string transportId, string baseStationId)
        {
            throw new NotImplementedException();
        }






        private async Task<List<HSE_transport_manager.Common.Models.TrainSchedulesData.TrainStop>> ConvertStopListAsync(
            TrainThreadInfoResponse trainThread)
        {
            var StopList = new List<HSE_transport_manager.Common.Models.TrainSchedulesData.TrainStop>();
            foreach (var stop in trainThread.TrainStops)
            {
                StopList.Add(new HSE_transport_manager.Common.Models.TrainSchedulesData.TrainStop
                {
                    ArrivalTime = stop.ArrivalTime == null ? trainThread.StartTime : (DateTime)stop.ArrivalTime,
                    ElapsedTime = new DateTime().AddSeconds(stop.TripDuration == null ? 0 : (double)stop.TripDuration),
                    StationCode = stop.StopStation.StationCode.YandexStationCode
                });
            }
            return StopList;
        }
    }
}
