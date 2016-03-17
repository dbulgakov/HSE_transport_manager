using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSE_transport_manager.Common.Models.TrainSchedulesData;

namespace HSE_transport_manager.Common.Interfaces
{
    public interface ITransportSchedulerService
    {
        Task<DailyTrainSchedule> GetDailyScheduleAsync(string startingStationCode, string endingStationCode);
        Task<SingleTrainSchedule> GetScheduleAsync(string transportId);
    }
}
