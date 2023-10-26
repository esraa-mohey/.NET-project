using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Dto.Stations
{
    public class StationRequest
    {
        public int Id { get; set; }

        public int OperationCode { get; set; }

        public int RegionId { get; set; }

        public int GovernorateId { get; set; }
        public string EnName { get; set; }

        public string ArName { get; set; }
    }
}
 