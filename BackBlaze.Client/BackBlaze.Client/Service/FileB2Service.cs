using BackBlaze.Client.IService;
using BackBlaze.Client.IService.Models.Request;
using BackBlaze.Client.IService.Models.Response;
using BackBlaze.Client.Options;
using Http.Client.Common;
using Http.Client.IService;
using Http.Client.Options;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.Json;

namespace BackBlaze.Client.Service
{
    /// <summary>
    /// сервис для работы с файлами googlrDisk
    /// </summary>
    public class FileB2Service : IFileB2Service
    {
        private IHttpClient _httpClient;
        private IBackblazeAuthService _backblazeAuthService;
        private BackBazeB2Options _options;
        private readonly HttpClientOptions _httpClientOptions;

        public FileB2Service(IBackblazeAuthService backblazeAuthService, IOptions<BackBazeB2Options> options,IOptions<HttpClientOptions> optionsHttp)
        {
            _httpClientOptions = optionsHttp.Value;
            _options = options.Value;
            _backblazeAuthService = backblazeAuthService;
        }

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-cancel-large-file
        /// </summary>
        public async Task<BaseResponse<B2CancelLargeFileResponse>> B2CancelLargeFile(B2CancelLargeFileRequest request)
        {
            var auth = await _backblazeAuthService.GetAuthAsync();

            _httpClient = new Http.Client.Service.HttpClient(auth.apiInfo.storageApi.apiUrl, new Http.Client.Models.Header
            {
                TypeAuth = "",
                AccessToken = auth.authorizationToken,
            },_httpClientOptions);

            var response = await _httpClient.SendAsync<B2CancelLargeFileResponse, B2CancelLargeFileRequest>("/b2api/v4/b2_cancel_large_file", HttpMethod.Post, request);

            return response;
        }

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-copy-file
        /// </summary>
        public async Task<BaseResponse<B2CopyFileResponse>> B2CopyFile(B2CopyFileRequest request)
        {
            var auth = await _backblazeAuthService.GetAuthAsync();

            _httpClient = new Http.Client.Service.HttpClient(auth.apiInfo.storageApi.apiUrl, new Http.Client.Models.Header
            {
                TypeAuth = "",
                AccessToken = auth.authorizationToken,
            }, _httpClientOptions);

            var response = await _httpClient.SendAsync<B2CopyFileResponse, B2CopyFileRequest>("/b2api/v4/b2_copy_file", HttpMethod.Post, request);

            return response;
        }

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-download-file-by-id
        /// </summary>
        public async Task<BaseResponse<B2DownloadFileByIdResponse>> B2DownloadFileById(B2DownloadFileByIdRequest request)
        {
            var response = new BaseResponse<B2DownloadFileByIdResponse>();

            try
            {
                var auth = await _backblazeAuthService.GetAuthAsync();

                var url = $"{auth.apiInfo.storageApi.apiUrl}/b2api/v4/b2_download_file_by_id?fileId={Uri.EscapeDataString(request.fileId)}";

                using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);

                httpRequest.Headers.Add("Authorization", auth.authorizationToken);

                using var httpClient = new System.Net.Http.HttpClient();
                using var httpResponse = await httpClient.SendAsync(httpRequest);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var fileContent = await httpResponse.Content.ReadAsByteArrayAsync();

                    response.Data = new B2DownloadFileByIdResponse
                    {
                        fileContent = fileContent,
                        contentType = httpResponse.Content.Headers.ContentType?.ToString() ?? "",
                        contentLength = httpResponse.Content.Headers.ContentLength,
                        fileId = GetHeaderValue(httpResponse, "X-Bz-File-Id"),
                        fileName = GetHeaderValue(httpResponse, "X-Bz-File-Name"),
                        contentSha1 = GetHeaderValue(httpResponse, "X-Bz-Content-Sha1"),
                        uploadTimestamp = GetHeaderValue(httpResponse, "X-Bz-Upload-Timestamp"),
                        contentDisposition = GetHeaderValue(httpResponse, "Content-Disposition")
                    };
                }
                else
                {
                    response.ErrorMessage = await httpResponse.Content.ReadAsStringAsync();
                }

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-download-file-by-name
        /// </summary>
        public async Task<BaseResponse<B2DownloadFileByNameResponse>> B2DownloadFileByName(B2DownloadFileByNameRequest request)
        {
            var auth = await _backblazeAuthService.GetAuthAsync();

            _httpClient = new Http.Client.Service.HttpClient(auth.apiInfo.storageApi.apiUrl, new Http.Client.Models.Header
            {
                TypeAuth = "",
                AccessToken = auth.authorizationToken,
            }, _httpClientOptions);

            var response = await _httpClient.SendAsync<B2DownloadFileByNameResponse, B2DownloadFileByNameRequest>($"/file/{request.bucketName}/{request.fileName}", HttpMethod.Get, request);

            return response;
        }

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-finish-large-file
        /// </summary>
        public async Task<BaseResponse<B2FinishLargeFileResponse>> B2FinishLargeFile(B2FinishLargeFileRequest request)
        {
            var auth = await _backblazeAuthService.GetAuthAsync();

            _httpClient = new Http.Client.Service.HttpClient(auth.apiInfo.storageApi.apiUrl, new Http.Client.Models.Header
            {
                TypeAuth = "",
                AccessToken = auth.authorizationToken,
            }, _httpClientOptions);

            var response = await _httpClient.SendAsync<B2FinishLargeFileResponse, B2FinishLargeFileRequest>("/b2api/v4/b2_finish_large_file", HttpMethod.Post, request);

            return response;
        }

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-get-file-info
        /// </summary>
        public async Task<BaseResponse<B2GetFileInfoResponse>> B2GetFileInfo(B2GetFileInfoRequest request)
        {
            var auth = await _backblazeAuthService.GetAuthAsync();

            _httpClient = new Http.Client.Service.HttpClient(auth.apiInfo.storageApi.apiUrl, new Http.Client.Models.Header
            {
                TypeAuth = "",
                AccessToken = auth.authorizationToken,
            }, _httpClientOptions);

            var response = await _httpClient.SendAsync<B2GetFileInfoResponse, object?>($"/b2api/v4/b2_get_file_info?fileId={request.fileId}", HttpMethod.Get, null);

            return response;
        }

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-list-file-names
        /// </summary>
        public async Task<BaseResponse<B2ListFileNamesResponse>> B2ListFileNames(B2ListFileNamesRequest request)
        {
            var auth = await _backblazeAuthService.GetAuthAsync();

            _httpClient = new Http.Client.Service.HttpClient(auth.apiInfo.storageApi.apiUrl, new Http.Client.Models.Header
            {
                TypeAuth = "",
                AccessToken = auth.authorizationToken,
            }, _httpClientOptions);

            var response = await _httpClient.SendAsync<B2ListFileNamesResponse, B2ListFileNamesRequest>("/b2api/v4/b2_list_file_names", HttpMethod.Post, request);

            return response;
        }

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-list-file-versions
        /// </summary>
        public async Task<BaseResponse<B2ListFileVersionsResponse>> B2ListFileVersions(B2ListFileVersionsRequest request)
        {
            var auth = await _backblazeAuthService.GetAuthAsync();

            _httpClient = new Http.Client.Service.HttpClient(auth.apiInfo.storageApi.apiUrl, new Http.Client.Models.Header
            {
                TypeAuth = "",
                AccessToken = auth.authorizationToken,
            }, _httpClientOptions);

            var response = await _httpClient.SendAsync<B2ListFileVersionsResponse, B2ListFileVersionsRequest>("/b2api/v4/b2_list_file_versions", HttpMethod.Post, request);

            return response;
        }

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-start-large-file
        /// </summary>
        public async Task<BaseResponse<B2StartLargeFileResponse>> B2StartLargeFile(B2StartLargeFileRequest request)
        {
            var auth = await _backblazeAuthService.GetAuthAsync();

            _httpClient = new Http.Client.Service.HttpClient(auth.apiInfo.storageApi.apiUrl, new Http.Client.Models.Header
            {
                TypeAuth = "",
                AccessToken = auth.authorizationToken,
            }, _httpClientOptions);

            var response = await _httpClient.SendAsync<B2StartLargeFileResponse, B2StartLargeFileRequest>("/b2api/v4/b2_start_large_file", HttpMethod.Post, request);

            return response;
        }

        public async Task<BaseResponse<B2UploadPartResponse>> B2UploadPart(B2UploadPartRequest request)
        {
            try
            {
                var uploadUrl = await GetUploadPartUrl(new GetUploadPartUrlRequest
                {
                    fileId = request.fileId
                });

                if (!uploadUrl.IsSusses)
                {
                    throw new Exception(uploadUrl.ErrorMessage);
                }

                var fileBytes = Convert.FromBase64String(request.base64);
                var sha1 = ComputeSha1Hex(fileBytes);

                using var httpClient = new HttpClient();
                using var requestHttp = new HttpRequestMessage(HttpMethod.Post, uploadUrl.Data!.uploadUrl);

                requestHttp.Headers.TryAddWithoutValidation("Authorization", uploadUrl.Data.authorizationToken);

                requestHttp.Headers.Add("X-Bz-Part-Number", request.partNumber.ToString());

                requestHttp.Headers.Add("X-Bz-Content-Sha1", sha1);

                requestHttp.Content = new ByteArrayContent(fileBytes);

                requestHttp.Content.Headers.ContentLength = fileBytes.Length;

                var response = await httpClient.SendAsync(requestHttp);

                if (!response.IsSuccessStatusCode)
                {
                    var errorJson = await response.Content.ReadAsStringAsync();
                    throw new Exception($"B2 UploadPart Error: {response.StatusCode} - {errorJson}");
                }

                var json = await response.Content.ReadAsStringAsync();
                var responseHttp = JsonSerializer.Deserialize<B2UploadPartResponse>(json)!;

                return new BaseResponse<B2UploadPartResponse>(responseHttp);
            }
            catch (Exception ex)
            {
                return new BaseResponse<B2UploadPartResponse>(ex.Message);
            }
        }

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-delete-file-version
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse<B2DeleteFileVersionResponse>> B2DeleteFileVersion(B2DeleteFileVersionRequest request)
        {
            var auth = await _backblazeAuthService.GetAuthAsync();

            _httpClient = new Http.Client.Service.HttpClient(auth.apiInfo.storageApi.apiUrl, new Http.Client.Models.Header
            {
                TypeAuth = "",
                AccessToken = auth.authorizationToken,
            }, _httpClientOptions);

            var response = await _httpClient.SendAsync<B2DeleteFileVersionResponse, B2DeleteFileVersionRequest>("/b2api/v4/b2_delete_file_version", HttpMethod.Post, request);

            return response;
        }

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-upload-file
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse<B2UploadFileResponse>> B2UploadFile(B2UploadFileRequest request)
        {
            try
            {
                var uploadUrl = await GetUploadUrl(new GetUploadUrlRequest
                {
                    bucketId = _options.BasketId
                });

                if (!uploadUrl.IsSusses)
                {
                    throw new Exception(uploadUrl.ErrorMessage);
                }

                var fileBytes = Convert.FromBase64String(request.base64);

                var sha1 = ComputeSha1Hex(fileBytes);

                using var httpClient = new HttpClient();
                using var requestHttp = new HttpRequestMessage(HttpMethod.Post, uploadUrl.Data!.uploadUrl);

                // Авторизация
                requestHttp.Headers.TryAddWithoutValidation("Authorization", uploadUrl.Data.authorizationToken);

                // Заголовки B2
                requestHttp.Headers.Add("X-Bz-File-Name", Uri.EscapeDataString(request.fileName));
                requestHttp.Headers.Add("X-Bz-Content-Sha1", sha1);

                // Контент
                requestHttp.Content = new ByteArrayContent(fileBytes);
                requestHttp.Content.Headers.ContentType = new MediaTypeHeaderValue("b2/x-auto");
                requestHttp.Content.Headers.ContentLength = fileBytes.Length;

                var response = await httpClient.SendAsync(requestHttp);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var responseHttp = JsonSerializer.Deserialize<B2UploadFileResponse>(json)!;

                return new BaseResponse<B2UploadFileResponse>(responseHttp);
            }
            catch (Exception ex)
            {
                return new BaseResponse<B2UploadFileResponse>(ex.Message);
            }
        }

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-get-upload-url
        /// </summary>
        /// <returns></returns>
        private async Task<BaseResponse<GetUploadUrlResponse>> GetUploadUrl(GetUploadUrlRequest request)
        {
            try
            {
                var auth = await _backblazeAuthService.GetAuthAsync();

                _httpClient = new Http.Client.Service.HttpClient(auth.apiInfo.storageApi.apiUrl, new Http.Client.Models.Header
                {
                    AccessToken = auth.authorizationToken,
                }, _httpClientOptions);

                var response = await _httpClient.SendAsync<GetUploadUrlResponse, GetUploadUrlRequest>("/b2api/v4/b2_get_upload_url", HttpMethod.Post, request);

                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponse<GetUploadUrlResponse>(ex.Message);
            }
        }

        /// <summary>
        /// https://www.backblaze.com/apidocs/b2-get-upload-part-url
        /// </summary>
        /// <returns></returns>
        private async Task<BaseResponse<GetUploadPartUrlResponse>> GetUploadPartUrl(GetUploadPartUrlRequest request)
        {
            try
            {
                var auth = await _backblazeAuthService.GetAuthAsync();

                _httpClient = new Http.Client.Service.HttpClient(auth.apiInfo.storageApi.apiUrl, new Http.Client.Models.Header
                {
                    AccessToken = auth.authorizationToken,
                }, _httpClientOptions);

                var response = await _httpClient.SendAsync<GetUploadPartUrlResponse, object?>($"/b2api/v4/b2_get_upload_part_url?fileId={request.fileId}", HttpMethod.Get, null);

                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponse<GetUploadPartUrlResponse>(ex.Message);
            }
        }

        private static string ComputeSha1Hex(byte[] data)
        {
            using var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(data);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        private string GetHeaderValue(HttpResponseMessage response, string headerName)
        {
            return response.Headers.TryGetValues(headerName, out var values)
                ? string.Join(", ", values)
                : "";
        }
    }
}
