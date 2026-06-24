using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackBlaze.Client.IService.Models.Response
{
    public class B2DownloadFileByNameResponse
    {
        public Stream ContentStream { get; set; } = null!;
        public HttpResponseMessage HttpResponse { get; set; } = null!; // Храним ссылку для корректного закрытия соединения

        // Заголовки ответа B2
        public string ContentType { get; set; } = null!;
        public long? ContentLength { get; set; }
        public string? FileId { get; set; }
        public string? FileName { get; set; }
        public string? ContentSha1 { get; set; }
        public string? UploadTimestamp { get; set; }
        public string? ContentDisposition { get; set; }
        public string? ServerSideEncryption { get; set; }

        public void Dispose()
        {
            ContentStream?.Dispose();
            HttpResponse?.Dispose();
        }
    }
}
