using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Dto.Response
{
    public class ResponseHeader
    {
        public int ResponseCode { get; set; }

        public string responseMessage { get; set; }

        public string ResponseRemark { get; set; }
    }
}
