using System.ComponentModel;

namespace HSE_transport_manager.Common.Interfaces
{
    public enum Transport
    {
        [Description("Электричка")]
        Suburban,
        [Description("Экспресс")]
        ExpressSuburban,
        Tram,
        Bus,
        Taxi
    }

    public interface ITransportService
    {
        void Initialize(string authKey);
    }
}
