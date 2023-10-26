using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERNST.Dto.Time_Table_Stations;
using ERNST.Model;

namespace ERNST.Dto
{
    public class ScheduleDto
    {
        public int Id { get; set; }

        public string StartDate { get; set; }
        public string ExpireDate { get; set; }
        public List<TimeTableStationForUpdate> TimeTableStations { get; set; }
        public bool Suspended { get; set; }
        public int LineId { get; set; }
        public string TrainNumber { get; set; }
        public int TrainId { get; set; }

    }
}
