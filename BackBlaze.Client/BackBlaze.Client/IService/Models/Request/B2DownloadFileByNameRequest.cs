using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackBlaze.Client.IService.Models.Request
{
    public class B2DownloadFileByNameRequest
    {
        public string bucketName { get; set; }

        public string fileName { get; set; }
    }
}
