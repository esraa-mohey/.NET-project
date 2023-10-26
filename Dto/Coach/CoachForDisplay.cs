using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Dto.Coach
{
    public class CoachForDisplay
    {
        public int Id { get; set; }
        public int CoachClassId { get; set; }
        public string CoachClassEnName { get; set; }
        public string CoachClassArName { get; set; }
        public string EnName { get; set; }
        public string ArName { get; set; }
        public int SeatsCount { get; set; }
        public int RowsCount { get; set; }
        public int ColsCount { get; set; }
        public string SeatsNumber { get; set; }
        public int CountOfCoaches { get; set; }
        public bool Suspended { get; set; }
    }
}
