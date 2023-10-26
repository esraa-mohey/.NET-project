using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Model
{
    public class Composition
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "this field is accept english characters only !")]
        public string  EnName { get; set; }


        [Required]
        [RegularExpression(@"^[\u0621-\u064A\u0660-\u0669 ]+$", ErrorMessage = "this field is accept arabic characters only !")]
        public string ArName { get; set; }
    }
}
