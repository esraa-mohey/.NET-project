using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERNST.Model
{
    public class Governorate
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string EnName { get; set; }

        [Required]
        public string ArName { get; set; }
    }
}
