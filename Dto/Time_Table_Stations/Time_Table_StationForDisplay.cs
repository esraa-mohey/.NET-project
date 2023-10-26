using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Dto.Time_Table_Stations
{
    public class Time_Table_StationForDisplay
    {
        public int Id { get; set; }
        public int LineStationsId { get; set; }
        public string Departure_time { get; set; }
        public string Arrival_time { get; set; }
        public bool Dci { get; set; }
    }
}
