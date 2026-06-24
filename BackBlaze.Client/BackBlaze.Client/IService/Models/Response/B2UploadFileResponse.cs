namespace BackBlaze.Client.IService.Models.Response
{
    public class B2UploadFileResponse
    {
        public string fileId { get; set; } = default!;
        public string fileName { get; set; } = default!;
        public string bucketId { get; set; } = default!;
    }
}
