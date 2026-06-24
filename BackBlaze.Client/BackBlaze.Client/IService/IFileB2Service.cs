using BackBlaze.Client.IService.Models.Request;
using BackBlaze.Client.IService.Models.Response;
using Http.Client.Common;

namespace BackBlaze.Client.IService
{
    /// <summary>
    /// Сервис для работы с файлами BackBlaze B2
    /// </summary>
    public interface IFileB2Service
    {
        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-cancel-large-file
        /// </summary>
        Task<BaseResponse<B2CancelLargeFileResponse>> B2CancelLargeFile(B2CancelLargeFileRequest request);

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-copy-file
        /// </summary>
        Task<BaseResponse<B2CopyFileResponse>> B2CopyFile(B2CopyFileRequest request);

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-download-file-by-id
        /// </summary>
        Task<BaseResponse<B2DownloadFileByIdResponse>> B2DownloadFileById(B2DownloadFileByIdRequest request);

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-download-file-by-name
        /// </summary>
        Task<BaseResponse<B2DownloadFileByNameResponse>> B2DownloadFileByName(B2DownloadFileByNameRequest request);

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-finish-large-file
        /// </summary>
        Task<BaseResponse<B2FinishLargeFileResponse>> B2FinishLargeFile(B2FinishLargeFileRequest request);

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-get-file-info
        /// </summary>
        Task<BaseResponse<B2GetFileInfoResponse>> B2GetFileInfo(B2GetFileInfoRequest request);

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-list-file-names
        /// </summary>
        Task<BaseResponse<B2ListFileNamesResponse>> B2ListFileNames(B2ListFileNamesRequest request);

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-list-file-versions
        /// </summary>
        Task<BaseResponse<B2ListFileVersionsResponse>> B2ListFileVersions(B2ListFileVersionsRequest request);

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-start-large-file
        /// </summary>
        Task<BaseResponse<B2StartLargeFileResponse>> B2StartLargeFile(B2StartLargeFileRequest request);

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-upload-part
        /// </summary>
        Task<BaseResponse<B2UploadPartResponse>> B2UploadPart(B2UploadPartRequest request);

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-delete-file-version
        /// </summary>
        Task<BaseResponse<B2DeleteFileVersionResponse>> B2DeleteFileVersion(B2DeleteFileVersionRequest request);

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-upload-file
        /// </summary>
        Task<BaseResponse<B2UploadFileResponse>> B2UploadFile(B2UploadFileRequest request);
    }
}
