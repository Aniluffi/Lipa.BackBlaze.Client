using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackBlaze.Client.IService.Models.Response
{
    public class B2DownloadFileByIdResponse
    {
        public byte[] fileContent { get; set; }
        public long? contentLength { get; set; }
        public string contentType { get; set; }
        public string fileId { get; set; }
        public string fileName { get; set; }
        public string contentSha1 { get; set; }
        public string uploadTimestamp { get; set; }
        public string contentDisposition { get; set; }
        public string contentLanguage { get; set; }
        public string expires { get; set; }
        public string cacheControl { get; set; }
        public string contentEncoding { get; set; }
        public string serverSideEncryption { get; set; }
        public string serverSideEncryptionCustomerAlgorithm { get; set; }
        public string serverSideEncryptionCustomerKeyMd5 { get; set; }
        public string fileRetentionMode { get; set; }
        public string fileRetentionRetainUntilTimestamp { get; set; }
        public string fileLegalHold { get; set; }
        public string clientUnauthorizedToRead { get; set; }
    }
}
