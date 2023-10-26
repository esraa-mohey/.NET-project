using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace ERNST.Dto.Stations
{
    public class StationsForDisplay
    {

        public int Id { get; set; }

        public int GovRegionsId { get; set; }

        public int GovernorateId { get; set; }

        public string GovernorateName { get; set; }
        public string RegionName { get; set; }
        public int RegionId { get; set; }

        public string EnName { get; set; }

        public string ArName { get; set; }
        public int OperationCode { get; set; }

    }
}
