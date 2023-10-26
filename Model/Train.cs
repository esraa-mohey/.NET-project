using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ERNST.Helper;
using ERNST.Helper.CustomValidation;

namespace ERNST.Model
{
    public class Train
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public TrainType TrainType { get; set; }
        public int TrainTypeId { get; set; }

        public Composition Composition { get; set; }
        public int CompositionId { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        [RegularExpression(@"^([0]?[1-9]|[1][0-2])[./-]([0]?[1-9]|[1|2][0-9]|[3][0|1])[./-]([0-9]{4}|[0-9]{2})$",
                                ErrorMessage = "this field is accept MM/dd/yyyy format !")]
        public string StartDate { get; set; }

        [Required]
        [RegularExpression(@"^([0]?[1-9]|[1][0-2])[./-]([0]?[1-9]|[1|2][0-9]|[3][0|1])[./-]([0-9]{4}|[0-9]{2})$",
                                ErrorMessage = "this field is accept MM/dd/yyyy format !")]
        
        public string EndDate { get; set; }

        [Required]
        public bool Suspended { get; set; }
    }
}
