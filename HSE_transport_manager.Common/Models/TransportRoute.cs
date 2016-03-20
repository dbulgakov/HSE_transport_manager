using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSDatabaseService.Models
{
    public class TransportRoute
    {
        public string FromPoint { get; set; }
        public string ToPoint { get; set; }
        public string TransportType { get; set; }
        public int Number { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ElapsedTime { get; set; }
        public double Price { get; set; }
    }
}
