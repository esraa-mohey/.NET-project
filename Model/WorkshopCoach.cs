using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ERNST.Model
{
    public class WorkshopCoach
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Coach Coach { get; set; }
        public int CoachId { get; set; }

        [Required]
        public int Available { get; set; }

        [Required]
        public int InUse { get; set; }

        [Required]
        public int InMaintenance { get; set; }
    }
}
