using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Dto.Time_Table_Stations
{
    public class Add_time_table
    {
        public int Time_Table_Id { get; set; }
        public List<Time_Table_StationForDisplay> time_Table_StationForDisplays { get; set; }

    }
}
