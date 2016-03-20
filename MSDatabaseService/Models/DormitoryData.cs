using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSDatabaseService.Models
{
    public class DormitoryData
    {
        public string Name { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
        public List<string> SubwayStation { get; set; }
        public string LocalTrainStation { get; set; }
        public bool CheckDubkiBus { get; set; }
        public List<string> From { get; set; }
        public List<string> To { get; set; }
    }
}
