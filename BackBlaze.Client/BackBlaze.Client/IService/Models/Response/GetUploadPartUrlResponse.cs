using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackBlaze.Client.IService.Models.Response
{
    public class GetUploadPartUrlResponse
    {
        public string fileId { get; set; }

        public string uploadUrl { get; set; }

        public string authorizationToken { get; set; }
    }
}
