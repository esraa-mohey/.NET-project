using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Model
{
    public class CompositionRecord
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Composition Composition { get; set; }
        public int CompositionId { get; set; }

        public Coach Coach { get; set; }
        public int CoachId { get; set; }

    }
}
