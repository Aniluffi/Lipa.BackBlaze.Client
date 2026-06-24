using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackBlaze.Client.IService.Models.Request
{
    public class B2UploadPartRequest
    {
        public string fileId { get; set; }
        public string base64 { get; set; }
        public string fileName { get; set; }
        public int partNumber { get; set; }
    }
}
