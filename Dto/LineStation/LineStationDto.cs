using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Dto.LineStation
{
    public class LineStationDto
    {
        public int Id { get; set; }
        public int LinesId { get; set; }

        public List<int> StationsId { get; set; }




    }
}
