using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Dto.Lines
{
    public class LineListStation
    {
        public int Id { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }

        public List<StationsProperty> Stations { get; set; }
    }
}
