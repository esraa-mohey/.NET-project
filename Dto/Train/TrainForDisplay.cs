using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Dto.Train
{
    public class TrainForDisplay
    {
        public int Id { get; set; }
        public int TrainTypeId { get; set; }
        public string TrainTypeArName { get; set; }
        public string TrainTypeEnName { get; set; }
        public int CompositionId { get; set; }
        public string CompositionArName { get; set; }
        public string CompositionEnName { get; set; }
        public string Number { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool Suspended { get; set; }
    }
}
