using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackBlaze.Client.IService.Models.Response
{
    public class B2CopyFileResponse
    {
        public string accountId { get; set; }

        public string action { get; set; }

        public string bucketId { get; set; }

        public long contentLength { get; set; }

        public string contentSha1 { get; set; }

        public string contentMd5 { get; set; }

        public string contentType { get; set; }

        public string fileId { get; set; }

        public object fileInfo { get; set; }

        public string fileName { get; set; }    

        public FileRetention fileRetention { get; set; }

        public LegalHold legalHold { get; set; }

        public string replicationStatus { get; set; }

        public ServerSideEncryption serverSideEncryption { get; set; }

        public long uploadTimestamp { get; set; }
    }
}
