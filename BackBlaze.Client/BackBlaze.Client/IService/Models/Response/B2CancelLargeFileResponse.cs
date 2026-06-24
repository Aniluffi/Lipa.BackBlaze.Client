using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackBlaze.Client.IService.Models.Response
{
    public class B2CancelLargeFileResponse
    {
        public string fileId { get; set; }

        public string accountId { get; set; }

        public string bucketId { get; set; }

        public string fileName { get; set; }
    }
}
