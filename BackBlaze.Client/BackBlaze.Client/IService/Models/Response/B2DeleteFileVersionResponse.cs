namespace BackBlaze.Client.IService.Models.Response
{
    public class B2DeleteFileVersionResponse
    {
        /// <summary>
        /// номер файла
        /// </summary>
        public string fileId { get; set; }

        /// <summary>
        /// название файла
        /// </summary>
        public string fileName { get; set; }
    }
}
