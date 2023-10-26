using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Dto.CoachClass
{
    public class CoachClassForDisplay
    {
        public int Id { get; set; }

        public string EnName { get; set; }
        public string ArName { get; set; }
        public bool Suspended { get; set; }
    }
}
