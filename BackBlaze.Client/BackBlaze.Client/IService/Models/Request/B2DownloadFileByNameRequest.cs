using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackBlaze.Client.IService.Models.Request
{
    public class B2DownloadFileByNameRequest
    {
        // Path параметры
        public string BucketName { get; set; } = null!;
        public string FileName { get; set; } = null!;

        // Header параметры
        public string? Range { get; set; }
        public string? XBzServerSideEncryptionCustomerAlgorithm { get; set; }
        public string? XBzServerSideEncryptionCustomerKey { get; set; }
        public string? XBzServerSideEncryptionCustomerKeyMd5 { get; set; }

        // Query параметры (опциональные переопределения)
        public string? Authorization { get; set; }
        public string? B2CacheControl { get; set; }
        public string? B2ContentDisposition { get; set; }
        public string? B2ContentEncoding { get; set; }
        public string? B2ContentLanguage { get; set; }
        public string? B2ContentType { get; set; }
        public string? B2Expires { get; set; }
    }
}
