using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Model
{
    public class Gov_Regions
    {
        [Required]  
        public int Id { get; set; }

        public Region Region { get; set; }
        public int RegionId { get; set; }

        public Governorate Governorate { get; set; }
        public int GovernorateId { get; set; }

        [NotMapped]
        public string GovenName { get; set; }

        [NotMapped]
        public string GovarName { get; set; }

    }
}
