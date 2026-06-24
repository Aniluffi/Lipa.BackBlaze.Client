using BackBlaze.Client.IService;
using BackBlaze.Client.IService.Models.Request;
using BackBlaze.Client.Options;
using Microsoft.Extensions.Options;
using System.ComponentModel;

namespace BackBlaze.Client.Test
{
    public class FileServiceTest
    {
        private readonly IFileB2Service _fileService;

        private readonly BackBazeB2Options _backBazeB2Options;

        public FileServiceTest(IFileB2Service fileService, IOptions<BackBazeB2Options> options)
        {
            _fileService = fileService;
            _backBazeB2Options = options.Value;
        }

        [Fact]
        [Description("Тест отмены загрузки файлов ао чанкам")]
        public async Task B2CancelLargeFile()
        {
            var fileName = "photoVideo123.mp4";

            var start = await _fileService.B2StartLargeFile(new B2StartLargeFileRequest
            {
                bucketId = _backBazeB2Options.BasketId,
                fileName = fileName,
                contentType = "video/mp4"
            });

            Assert.True(start.IsSusses);

            var base64 = GetFile("chunk_base64.txt");

            var getChunks = SplitBase64ToChunks(base64, 5 * 1024 * 1024, start.Data.fileId, fileName);

            var partSha1Array = new List<string>();

            var uploadChunk = await _fileService.B2UploadPart(getChunks.FirstOrDefault());

            Assert.True(uploadChunk.IsSusses);

            partSha1Array.Add(uploadChunk.Data.contentSha1);

            var cancel = await _fileService.B2CancelLargeFile(new B2CancelLargeFileRequest
            {
                fileId = start.Data.fileId
            });

            Assert.True(cancel.IsSusses);
        }

        [Fact]
        [Description("тест копирования файла")]
        public async Task B2CopyFile()
        {
            var fileName = "photoVideo(1).mp4";

            var copyFile = await _fileService.B2CopyFile(new B2CopyFileRequest
            {
                fileName = fileName,
                sourceFileId = "4_z75fdcd1a4022faad9ea60415_f2069db7974c5700e_d20260503_m175813_c003_v0312025_t0009_u01777831093690"
            });

            Assert.True(copyFile.IsSusses);
        }

        [Fact]
        [Description("проверка скачки файла по номеру.")]
        public async Task B2DownloadFileById()
        {
            var downoloadFile = await _fileService.B2DownloadFileById(new B2DownloadFileByIdRequest
            {
                fileId = "4_z75fdcd1a4022faad9ea60415_f10511e421dabfb2d_d20260515_m143749_c003_v0312036_t0028_u01778855869134",
            });

            Assert.True(downoloadFile.IsSusses);
        }

        [Fact]
        [Description("проверка скачки файла по названию.")]
        public async Task B2DownloadFileByName()
        {
            var downoloadFile = await _fileService.B2DownloadFileByName(new B2DownloadFileByNameRequest
            {
                fileName = "mashina_neon_podsvetka_158672_3840x2160.jpg",
                bucketName = "staffIofiles"
            });

            Assert.True(downoloadFile.IsSusses);
        }

        [Fact]
        [Description("Тест на получение информации о файле.")]
        public async Task B2GetFileInfo()
        {
            var getFile = await _fileService.B2GetFileInfo(new B2GetFileInfoRequest
            {
                fileId = "4_z75fdcd1a4022faad9ea60415_f2069db7974c5700e_d20260503_m175813_c003_v0312025_t0009_u01777831093690"
            });

            Assert.True(getFile.IsSusses);
        }

        [Fact]
        [Description("Тест получения названий файлов")]
        public async Task B2ListFileNames()
        {
            var getList = await _fileService.B2ListFileNames(new B2ListFileNamesRequest
            {
                bucketId = _backBazeB2Options.BasketId
            });

            Assert.True(getList.IsSusses);
        }

        [Fact]
        [Description("проверка получения списка версий файла.")]
        public async Task B2ListFileVersions()
        {
            var fileName = "photoVideo.mp4";

            var getList = await _fileService.B2ListFileVersions(new B2ListFileVersionsRequest
            {
                bucketId = _backBazeB2Options.BasketId,
            });

            Assert.True(getList.IsSusses);
        }

        [Fact]
        [Description("Проверка на загрузку по частям")]
        public async Task UploadLargeFile()
        {
            var fileName = "photoVideo123.mp4";

            var start = await _fileService.B2StartLargeFile(new B2StartLargeFileRequest
            {
                bucketId = _backBazeB2Options.BasketId,
                fileName = fileName,
                contentType = "video/mp4"
            });

            Assert.True(start.IsSusses);

            var base64 = GetFile("chunk_base64.txt");

            var getChunks = SplitBase64ToChunks(base64, 5 * 1024 * 1024, start.Data.fileId, fileName);

            var partSha1Array = new List<string>();

            foreach (var chunk in getChunks)
            {
                var uploadChunk = await _fileService.B2UploadPart(chunk);

                Assert.True(uploadChunk.IsSusses);

                partSha1Array.Add(uploadChunk.Data.contentSha1);
            }

            var finish = await _fileService.B2FinishLargeFile(new B2FinishLargeFileRequest
            {
                fileId = start.Data.fileId,
                partSha1Array = partSha1Array
            });

            Assert.True(finish.IsSusses);
        }

        [Fact]
        [Description("проверка на создание файла")]
        public async Task Createfile()
        {
            var createFile = await _fileService.B2UploadFile(new B2UploadFileRequest
            {
                fileName = "photo.jpg",
                base64 = GetFile()
            });

            Assert.True(!string.IsNullOrWhiteSpace(createFile.Data.fileId));
        }

        [Fact]
        [Description("проверка на удаление файла")]
        public async Task DeleteFile()
        {
            var fileName = "photo2.png";

            var createFile = await _fileService.B2UploadFile(new B2UploadFileRequest
            {
                fileName = fileName,
                base64 = GetFile()
            });

            if (!createFile.IsSusses)
                throw new Exception("Не удалось загрузить файл.");

            var deleteFile = await _fileService.B2DeleteFileVersion(new B2DeleteFileVersionRequest
            {
                fileId = createFile.Data.fileId,
                fileName = fileName
            });

            Assert.True(deleteFile.IsSusses);
        }

        /// <summary>
        /// получение base64
        /// </summary>
        /// <returns></returns>
        private string GetFile(string name = "base64.txt")
        {
            var base64 = File.ReadAllText(name);
            return base64;
        }

        public List<B2UploadPartRequest> SplitBase64ToChunks(string base64, int chunkSizeBytes, string fileId, string fileName)
        {
            if (string.IsNullOrWhiteSpace(base64))
                throw new ArgumentException("Base64 string is empty");

            // Минимум 5,000,000 байт для Backblaze B2
            const int MIN_B2_SIZE = 5000000;
            if (chunkSizeBytes < MIN_B2_SIZE) chunkSizeBytes = MIN_B2_SIZE;

            var bytes = Convert.FromBase64String(base64);
            var requests = new List<B2UploadPartRequest>();
            int partNumber = 1; // Нумерация частей в B2 начинается с 1

            for (int i = 0; i < bytes.Length; i += chunkSizeBytes)
            {
                int size = Math.Min(chunkSizeBytes, bytes.Length - i);
                var chunk = new byte[size];
                Array.Copy(bytes, i, chunk, 0, size);

                requests.Add(new B2UploadPartRequest
                {
                    fileId = fileId,
                    fileName = fileName,
                    partNumber = partNumber++, // Увеличиваем номер для каждой следующей части
                    base64 = Convert.ToBase64String(chunk)
                });
            }

            return requests;
        }
    }
}