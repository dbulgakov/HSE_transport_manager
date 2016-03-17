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
using YandexScheduleService.DTO.Response.ThreadListResponse;

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
            return await Task<DailyTrainSchedule>.Factory.StartNew(() =>
            {
                var scheduledTrains = new List<SingleTrainSchedule>();
                var requestString = _requestBuilder.ThreadsListRequest(startingStationCode, endingStationCode);
                HttpClient client = new HttpClient();
                var responseString = client.GetAsync(requestString).Result.Content.ReadAsStringAsync().Result;
                var threadListResponse = JsonConvert.DeserializeObject<TrainThreadsListResponse>(responseString);

                foreach (var train in threadListResponse.TrainThreadList)
                {
                    scheduledTrains.Add(GetScheduleAsync(train.TrainInfo.TrainUid).Result);
                }


                var dailyTrainSchedule = new DailyTrainSchedule
                {
                    DepartureStation = startingStationCode,
                    ArrivalStation = endingStationCode,
                    ScheduledTrains = scheduledTrains
                };

                return dailyTrainSchedule;
            });
        }

        public async Task<SingleTrainSchedule> GetScheduleAsync(string transportId)
        {
            return await Task<SingleTrainSchedule>.Factory.StartNew(() =>
            {
                var requestString = _requestBuilder.ThreadInfoRequest(transportId);
                HttpClient client = new HttpClient();

                var responseString = client.GetAsync(requestString).Result.Content.ReadAsStringAsync().Result;
                var threadInfoResponse = JsonConvert.DeserializeObject<TrainThreadInfoResponse>(responseString);

                var trainStopList = ConvertStopList(threadInfoResponse);

                var trainSchedule = new SingleTrainSchedule
                {   
                    TrainUid = transportId,
                    DepartureTime = threadInfoResponse.StartTime,
                    Stops = trainStopList
                };
                return trainSchedule;
            });
        }


        public async Task<SingleTrainSchedule> GetScheduleAsync(string transportId, string baseStationId)
        {
            return await Task<SingleTrainSchedule>.Factory.StartNew(() =>
            {
                var requestString = _requestBuilder.ThreadInfoRequest(transportId);
                HttpClient client = new HttpClient();
                var responseString = client.GetAsync(requestString).Result.Content.ReadAsStringAsync().Result;
                var threadInfoResponse = JsonConvert.DeserializeObject<TrainThreadInfoResponse>(responseString);

                var trainStopList = ConvertStopList(threadInfoResponse);

                var trainSchedule = new SingleTrainSchedule
                {
                    TrainUid = transportId,
                    DepartureTime = threadInfoResponse.StartTime,
                    Stops = trainStopList
                };
                return trainSchedule;
            });
        }


        private List<HSE_transport_manager.Common.Models.TrainSchedulesData.TrainStop> ConvertStopList(
           TrainThreadInfoResponse trainThread, string baseStationId)
        {
            var stopList = new List<HSE_transport_manager.Common.Models.TrainSchedulesData.TrainStop>();
            var reachedBase = false;
            double elapsedTime = 0;
            foreach (var stop in trainThread.TrainStops)
            {         
                if (reachedBase)
                {
                    stopList.Add(new HSE_transport_manager.Common.Models.TrainSchedulesData.TrainStop
                    {
                        ArrivalTime = stop.ArrivalTime == null ? trainThread.StartTime : (DateTime)(stop.ArrivalTime),
                        ElapsedTime = new DateTime().AddSeconds(stop.TripDuration == null ? 0 : (double)stop.TripDuration - elapsedTime),
                        StationCode = stop.StopStation.StationCode.YandexStationCode
                    });
                }

                if (stop.StopStation.StationCode.YandexStationCode.Equals(baseStationId))
                {
                    reachedBase = true;
                    elapsedTime = elapsedTime + stop.TripDuration ?? (double)stop.TripDuration;
                }
            }
            return stopList;
        }


        private List<HSE_transport_manager.Common.Models.TrainSchedulesData.TrainStop> ConvertStopList(
           TrainThreadInfoResponse trainThread)
        {
            var StopList = new List<HSE_transport_manager.Common.Models.TrainSchedulesData.TrainStop>();
            foreach (var stop in trainThread.TrainStops)
            {
                StopList.Add(new HSE_transport_manager.Common.Models.TrainSchedulesData.TrainStop
                {
                    ArrivalTime = stop.ArrivalTime == null ? trainThread.StartTime : (DateTime) (stop.ArrivalTime),
                    ElapsedTime = new DateTime().AddSeconds(stop.TripDuration == null? 0 : (double) stop.TripDuration),
                    StationCode = stop.StopStation.StationCode.YandexStationCode
                });
            }
            return StopList;
        }
    }
}
