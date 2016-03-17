using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
