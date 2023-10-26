using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Model
{
    public class TrainSchedule
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int LineStationsId { get; set; }
        public LineStations LineStations { get; set; }
        public int TrainId { get; set; }
        public Train Train { get; set; }
        [Required]
        public string DepartureTime { get; set; }

        [Required]
        public string ArrivalTime { get; set; }



    }
}
