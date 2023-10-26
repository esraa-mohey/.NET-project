using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Dto.GovRegion
{
    public class GovRegionForDisplay
    {
        public int Id { get; set; }

        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public int GovernorateId { get; set; }
        public string GovarName { set; get; }
        public string GovenName { set; get; }


    }
}
