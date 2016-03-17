using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSE_transport_manager.Common.Interfaces;
using HSE_transport_manager.Common.Models.TrainSchedulesData;

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
        }

        public Task<DailyTrainSchedule> GetScheduleAsync(string startingStationCode, string endingStationCode)
        {
            throw new NotImplementedException();
        }
    }
}
