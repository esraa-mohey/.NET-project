using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Dto.LineStation
{
    public class GetLineStation
    {
        public int Id { get; set; }
        public int LineStationId { get; set; }
        public int LinesId { get; set; }
        public string LineAr { get; set; }
        public string LineEn { get; set; }
        public int OperationCode { get; set; }

        public int StationsId { get; set; }
        public string StationsEn { get; set; }
        public string StationsAr { get; set; }
    }
}
