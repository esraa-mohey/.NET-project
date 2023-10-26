using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Model
{
    public class Lines
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string EnName { get; set; }
        [Required]
        public string ArName { get; set; }

        [NotMapped]
        public string Stations { get; set; }

    }
}
