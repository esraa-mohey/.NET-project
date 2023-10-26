using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Model
{
    public class CoachClass
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[\u0621-\u064A0-9 ]+$", ErrorMessage = "this field is accept arabic characters only !")]
        public string ArName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "this field is accept english characters only !")]
        public string EnName { get; set; }

        [Required]
        public bool Suspended { get; set; }
    }
}
