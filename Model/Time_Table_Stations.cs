using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Model
{
    public class Time_Tabale_Stations
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int LineStationsId { get;set;}
        public LineStations LineStations { get;set;}
        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
        [Required]
        public string Departure_time { get; set; }
        [Required]
        public string Arrival_time { get; set; }
        [Required]
        public bool Dci { get; set; }

    }
}
