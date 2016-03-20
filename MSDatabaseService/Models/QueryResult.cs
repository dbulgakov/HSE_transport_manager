using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSDatabaseService.Models
{
    public class QueryResult
    {
        public string DeparturePoint { get; set; }
        public string ArrivalPoint { get; set; }
        public List<Route> Routes { get; set; }
    }
}
