using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Dto.Composition
{
    public class CompositionForDisplay
    {
        public int Id { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public string Coaches { get; set; }
        public bool IsEditable { get; set; }
    }
}
