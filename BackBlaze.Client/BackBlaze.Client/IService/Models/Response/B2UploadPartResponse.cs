using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackBlaze.Client.IService.Models.Response
{
    public class B2UploadPartResponse
    {
        public string fileId { get; set; }

        public int partNumber { get; set; }

        public int contentLength { get; set; }

        public string contentSha1 { get; set; }

        public string contentMd5 { get; set; }

        public ServerSideEncryption serverSideEncryption { get; set; }

        public long uploadTimestamp { get; set; }
    }
}
