﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSE_transport_manager.Common.Interfaces;
using HSE_transport_manager.Common.Models.TaxiData;

namespace UberService
{
    class UberService : ITransportService, ITaxiService
    {
        public void Initialize(string authKey)
        {
            throw new NotImplementedException();
        }

        public TaxiTripData GetRoute(Coordinate startingPoint, Coordinate endingPoint)
        {
            throw new NotImplementedException();
        }
    }
}
