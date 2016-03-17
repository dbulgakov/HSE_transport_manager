using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexScheduleService
{
    class RequestBuilder
    {
        private readonly string _apiUrl;
        private readonly string _apiVer;
        private readonly string _authKey;

        private const string ApiDateFormat = "yyyy-MM-dd";

        private const string ThreadsListRequestString =
            "search/?apikey={0}&from={1}&to={2}&date={3}&format=json&lang=ru";

        private const string ThreadInfoRequestString =
            "thread/?apikey={0}&uid={1}&date={2}&format=json&lang=ru&show_systems=yandex";

        public RequestBuilder(string apiUrl, string apiVer, string authKey)
        {
            _apiUrl = apiUrl;
            _apiVer = apiVer;
            _authKey = authKey;
        }

        public string ThreadsListRequest(string startingStation, string endingStation)
        {
            var param = string.Format(
                ThreadsListRequestString,
                _authKey,
                startingStation,
                endingStation,
                DateTime.Now.ToString(ApiDateFormat)
                );
            return string.Format("{0}/{1}/{2}", _apiUrl, _apiVer, param);
        }

        public string ThreadInfoRequest(string trainUid)
        {
            var param = string.Format(
                ThreadInfoRequestString,
                _authKey,
                trainUid,
                DateTime.Now.ToString(ApiDateFormat)
                );
            return string.Format("{0}/{1}/{2}", _apiUrl, _apiVer, param);
        }
    }
}
