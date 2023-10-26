using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Model
{
    public class Coach
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public CoachClass CoachClass { get; set; }
        public int CoachClassId { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "this field is accept english characters only !")]
        public string EnName { get; set; }

        [Required]
        [RegularExpression(@"^[\u0621-\u064A0-9 ]+$", ErrorMessage = "this field is accept arabic characters only !")]
        public string ArName { get; set; }

        [Required]
        public int SeatsCount { get; set; }

        [Required]
        public int RowsCount { get; set; }

        [Required]
        public int ColsCount { get; set; }

        public int SeatsNumber { get; set; }

        [Required]
        public int CountOfCoaches { get; set; }

        [Required]
        public bool Suspended { get; set; }
    }
}
