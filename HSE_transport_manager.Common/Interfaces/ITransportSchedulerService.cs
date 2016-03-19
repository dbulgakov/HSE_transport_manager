using System.Threading.Tasks;
using HSE_transport_manager.Common.Models.TrainSchedulesData;

namespace HSE_transport_manager.Common.Interfaces
{
    public interface ITransportSchedulerService
    {
        Task<DailyTrainSchedule> GetDailyScheduleAsync(string startingStationCode, string endingStationCode);
        Task<SingleTrainSchedule> GetScheduleAsync(string transportId, string baseStationId);
        Task<SingleTrainSchedule> GetScheduleAsync(string transportId);
    }
}
