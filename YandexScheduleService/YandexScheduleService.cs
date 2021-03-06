﻿using System;
using System.Collections.Generic;
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
    class YandexScheduleService : ITransportSchedulerService
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
            return await Task.Run(async () =>
            {
                var scheduledTrains = new List<SingleTrainSchedule>();
                var requestString = _requestBuilder.ThreadsListRequest(startingStationCode, endingStationCode);

                var client = new HttpClient();

                var responseString = client.GetAsync(requestString).Result.Content.ReadAsStringAsync().Result;
                var threadListResponse = JsonConvert.DeserializeObject<TrainThreadsListResponse>(responseString);

                foreach (var train in threadListResponse.TrainThreadList)
                {
                    var trainThread = await GetScheduleAsync(train.TrainInfo.TrainUid, startingStationCode,
                        train.TrainInfo.TrainExpressType, (DateTime)train.DepartureTime);
                    if (trainThread != null)
                        scheduledTrains.Add(trainThread);
                }

                return CreateDailyTrainSchedule(startingStationCode, endingStationCode, scheduledTrains);
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
                var responce = client.GetAsync(requestString).Result;
                if (!responce.IsSuccessStatusCode)
                    return null;
                var responseString = responce.Content.ReadAsStringAsync().Result;
                var threadInfoResponse = JsonConvert.DeserializeObject<TrainThreadInfoResponse>(responseString);

                var trainStopList = ConvertStopList(threadInfoResponse, baseStationId);
                return CreateTrainSchedule(transportId, departureTime, trainStopList, trainType);
            });
        }




        private List<TrainStop> ConvertStopList(
           TrainThreadInfoResponse trainThread, string baseStationId)
        {
            var stopList = new List<TrainStop>();
            var reachedBase = baseStationId == null;
            double elapsedTime = 0;
            foreach (var stop in trainThread.TrainStops)
            {
                if (reachedBase)
                {
                    stopList.Add(new TrainStop
                    {
                        ArrivalTime = stop.ArrivalTime ?? trainThread.StartTime,
                        ElapsedTime = new DateTime().AddSeconds(stop.TripDuration - elapsedTime ?? 0),
                        StationCode = stop.StopStation.StationCode.YandexStationCode
                    });
                }

                if (!stop.StopStation.StationCode.YandexStationCode.Equals(baseStationId)) continue;
                reachedBase = true;
                if (stop.TripDuration != null)
                    elapsedTime = elapsedTime + stop.TripDuration ?? (double)stop.TripDuration;
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


        private DailyTrainSchedule CreateDailyTrainSchedule(string startingStationCode,
            string endingStationCode, IList<SingleTrainSchedule> scheduledTrains)
        {
            return new DailyTrainSchedule
            {
                DepartureStation = startingStationCode,
                ArrivalStation = endingStationCode,
                ScheduledTrains = scheduledTrains
            };
        }
    }
}
