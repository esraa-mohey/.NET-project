using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Dto.LineStation
{
    public class LineStationUpdate
    {
        public int Id { get; set; }
        public int LinesId { get; set; }
        public string LineAr { get; set; }
        public string LineEn { get; set; }
        public List<int> StationsId { get; set; }

    }
}
