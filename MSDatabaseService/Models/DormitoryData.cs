﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSDatabaseService.Models
{
    class DormitoryData
    {
        public string[] Name { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Address { get; set; }
        public string[] SubwayStation { get; set; }
        public string LocalTrainStation { get; set; }
        public bool ChechDubkiBus { get; set; }
        public int[] From { get; set; }
        public int[] To { get; set; }
    }
}
