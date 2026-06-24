using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackBlaze.Client.IService.Models.Request
{
    public class B2StartLargeFileRequest
    {
        public string bucketId { get; set; }
        public string fileName { get; set; }
        public string contentType { get; set; }
        public string customUploadTimestamp { get; set; }
        public object fileInfo { get; set; }
        public FileRetention fileRetention { get; set; }
        public LegalHold legalHold { get; set; }

        public ServerSideEncryption serverSideEncryption { get; set; }
    }
}
