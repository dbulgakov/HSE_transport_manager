namespace HSE_transport_manager.Common.Interfaces
{
    public enum Transport
    {
        Suburban,
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
