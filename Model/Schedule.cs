using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ERNST.Dto.Time_Table_Stations;
using ERNST.Helper.CustomValidation;

namespace ERNST.Model
{
    public class Schedule
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^([0]?[1-9]|[1][0-2])[./-]([0]?[1-9]|[1|2][0-9]|[3][0|1])[./-]([0-9]{4}|[0-9]{2})$",
                                ErrorMessage = "this field is accept MM/dd/yyyy format !")]
        [CheckStartDateRange]
        public string StartDate { get; set; }
        [Required]
        [RegularExpression(@"^([0]?[1-9]|[1][0-2])[./-]([0]?[1-9]|[1|2][0-9]|[3][0|1])[./-]([0-9]{4}|[0-9]{2})$",
                                        ErrorMessage = "this field is accept MM/dd/yyyy format !")]
        [CheckStartDateRange] public string ExpireDate { get; set; }
        public bool Suspended { get; set; }

        [Required]
        public int TrainId { get; set; }
        public Train Train { get; set; }


        [NotMapped]
        public List<TimeTableStationForUpdate> TimeTableStations { get; set; }

        [NotMapped]
        public int LineId { get; set; }
    }
}
