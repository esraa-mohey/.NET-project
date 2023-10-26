using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Model
{
    public class LineStations
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int LinesId { get; set; }
        public Lines Lines { get; set; }

        public int StationsId { get; set; }
        public Stations Stations { get; set; }


    }
}
