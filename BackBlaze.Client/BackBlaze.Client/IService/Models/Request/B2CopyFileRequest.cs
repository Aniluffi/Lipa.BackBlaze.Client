namespace BackBlaze.Client.IService.Models.Request
{
    public class B2CopyFileRequest
    {
        public string sourceFileId { get; set; }

        public string destinationBucketId { get; set; }

        public string fileName { get; set; }

        public string range { get; set; }

        public string metadataDirective { get; set; }

        public string contentType { get; set; }

        public object fileInfo { get; set; }

        public WriteFileRetentions fileRetention { get; set; }

        public string legalHold { get; set; }

        public SourceServerSideEncryption sourceServerSideEncryption { get; set; }

        public DestinationServerSideEncryption destinationServerSideEncryption { get; set; }
    }
}
