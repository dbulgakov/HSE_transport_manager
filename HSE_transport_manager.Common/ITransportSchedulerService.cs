using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSE_transport_manager.Common.Models;

namespace HSE_transport_manager.Common
{
    interface ITransportSchedulerService
    {
        void Initialize();
        TripData GetRoute(string StartingStation, string EndingStation);
    }
}
