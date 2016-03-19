﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSE_transport_manager.Common.Models;

namespace HSE_transport_manager.Common.Interfaces
{
    public interface ITransportMonitoringService
    {
        int EstimateTripTime(Coordinate startingPoint, Coordinate endingPoint);
    }
}
