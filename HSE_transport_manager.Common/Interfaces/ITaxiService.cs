using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSE_transport_manager.Common.Models.TaxiData;

namespace HSE_transport_manager.Common
{
    interface ITaxiService
    {
        TripData GetRoute(Coordinate startingPoint, Coordinate endingPoint);
    }
}
