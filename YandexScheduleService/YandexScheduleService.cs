using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HSE_transport_manager.Common.Interfaces;
using HSE_transport_manager.Common.Models.TrainSchedulesData;
using Newtonsoft.Json;
using System.Net.Http;
using YandexScheduleService.DTO.Response.ThreadInfoResponse;
using YandexScheduleService.DTO.Response.ThreadListResponse;
using TrainStop = HSE_transport_manager.Common.Models.TrainSchedulesData.TrainStop;

namespace YandexScheduleService
{
    class YandexScheduleService: ITransportSchedulerService
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
            return await Task.Run(() =>
            {
                var scheduledTrains = new List<SingleTrainSchedule>();
                var requestString = _requestBuilder.ThreadsListRequest(startingStationCode, endingStationCode);

                var client = new HttpClient();

                var responseString = client.GetAsync(requestString).Result.Content.ReadAsStringAsync().Result;
                var threadListResponse = JsonConvert.DeserializeObject<TrainThreadsListResponse>(responseString);

                foreach (var train in threadListResponse.TrainThreadList)
                {
                    scheduledTrains.Add(GetScheduleAsync(train.TrainInfo.TrainUid, startingStationCode, train.TrainInfo.TrainExpressType, (DateTime)train.DepartureTime).Result);
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

        public async Task<SingleTrainSchedule> GetScheduleAsync(string transportId, string baseStationId = null)
        {
            return await Task.Run(() =>
            {
                var requestString = _requestBuilder.ThreadInfoRequest(transportId);
                var client = new HttpClient();
                var responseString = client.GetAsync(requestString).Result.Content.ReadAsStringAsync().Result;
                var threadInfoResponse = JsonConvert.DeserializeObject<TrainThreadInfoResponse>(responseString);

                var trainStopList = ConvertStopList(threadInfoResponse, baseStationId);
                return CreateTrainSchedule(transportId, threadInfoResponse.StartTime, trainStopList);
            });
        }


        private async Task<SingleTrainSchedule> GetScheduleAsync(string transportId, string baseStationId, string trainType, DateTime departureTime)
        {
            return await Task.Run(() =>
            {
                var requestString = _requestBuilder.ThreadInfoRequest(transportId);
                var client = new HttpClient();
                var responseString = client.GetAsync(requestString).Result.Content.ReadAsStringAsync().Result;
                var threadInfoResponse = JsonConvert.DeserializeObject<TrainThreadInfoResponse>(responseString);

                var trainStopList = ConvertStopList(threadInfoResponse, baseStationId);
                return CreateTrainSchedule(transportId, departureTime, trainStopList, trainType);
            });
        }




        private List<TrainStop> ConvertStopList(
           TrainThreadInfoResponse trainThread, string baseStationId)
        {
            var stopList = new List<TrainStop>();
            var reachedBase = false;
            double elapsedTime = 0;
            foreach (var stop in trainThread.TrainStops)
            {         
                if (reachedBase)
                {
                    stopList.Add(new TrainStop
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


        private List<TrainStop> ConvertStopList(
           TrainThreadInfoResponse trainThread)
        {
            var stopList = new List<HSE_transport_manager.Common.Models.TrainSchedulesData.TrainStop>();
            foreach (var stop in trainThread.TrainStops)
            {
                stopList.Add(new HSE_transport_manager.Common.Models.TrainSchedulesData.TrainStop
                {
                    ArrivalTime = stop.ArrivalTime ?? trainThread.StartTime,
                    ElapsedTime = new DateTime().AddSeconds(stop.TripDuration ?? 0),
                    StationCode = stop.StopStation.StationCode.YandexStationCode
                });
            }
            return stopList;
        }


        private SingleTrainSchedule CreateTrainSchedule(string transportId, DateTime departureTime,
            IList<TrainStop> stops, string trainType = null)
        {
            var trainSchedule = new SingleTrainSchedule
            {
                TrainUid = transportId,
                DepartureTime = departureTime,
                Stops = stops,
                TransportType = trainType == null ? Transport.Suburban : Transport.ExpressSuburban 
            };
            return trainSchedule;
        }
    }
}
