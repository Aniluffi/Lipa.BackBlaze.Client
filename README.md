# BackBlaze.Client

Асинхронный C# клиент для бесшовной интеграции с объектным хранилищем **Backblaze B2 Cloud Storage**. Библиотека предоставляет удобный и изолированный слой абстракции для работы с файлами без необходимости ручного написания HTTP-запросов.

## Основные возможности

* **Автоматическая авторизация:** Самостоятельный менеджмент и обновление сессионных токенов через `IBackblazeAuthService`.
* **Оптимизация под Large Files:** Поддержка поочередной загрузки тяжелых файлов (видео, архивы) чанками (от 5 МБ) с возможностью атомарной отмены сессии сборки.
**Стриминг без перегрузки RAM:** Метод скачивания файлов по имени возвращает оптимизированный поток данных (`Stream`), предотвращая лишнюю буферизацию данных в оперативной памяти.
   **Гибкая конфигурация HTTP:** Поддержка кастомных настроек таймаутов, повторных попыток (retries) и задержек выполнения HTTP-запросов.
  **Готовый DI-слой:** Быстрая регистрация всех компонентов в `IServiceCollection` одной командой.

---

## ?? Установка и конфигурация

### 1. Настройка параметров (`appsettings.json`)
Добавьте секции конфигурации для Backblaze B2 и сетевых параметров HTTP-клиента в ваш файл настроек:

```json
{
  "BackBazeB2Options": {
    "ApplicationKeyId": "5dda02ade645",
    "ApplicationKey": "0037a95de2a05a9a0f0d93e18ccb50b291362dbe37",
    "AuthPatch": "[https://api.backblazeb2.com/b2api/v4/b2_authorize_account](https://api.backblazeb2.com/b2api/v4/b2_authorize_account)",
    "BasketId": "75fdcd1a4022faad9ea60415"
  },
  "HttpClientOptions": {
    "TimeoutMs": 10000,
    "RetryCount": 2,
    "RetryDelayMs": 1000
  }
}

```

### 2. Регистрация сервисов в Dependency Injection

Подключите пространство имен `using BackBlaze.Client.Extensions;` и зарегистрируйте зависимости в конфигурации вашего приложения (`Program.cs` или кастомный `Startup`):

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Метод автоматически настраивает конфигурации HttpClientOptions, BackBazeB2Options
    // и регистрирует службы IBackblazeAuthService и IFileB2Service
    services.AddFileService(configuration);
}

```

---

## Примеры использования

### 1. Скачивание файла по названию (Потоковое чтение)

Метод скачивания оптимизирован под низкое потребление памяти и читает только заголовки ответа, отдавая сырой поток. **Обязательно** используйте конструкцию `using` для своевременного закрытия HTTP-клиента и сокетов:

```csharp
var request = new B2DownloadFileByNameRequest
{
    BucketName = "staffIofiles",
    FileName = "mashina_neon_podsvetka_158672_3840x2160.jpg"
};

var response = await _fileService.B2DownloadFileByName(request);

if (response.IsSusses && response.Data != null)
{
    // Освобождаем ресурсы HttpResponseMessage и HttpClient сразу после вычитки стрима
    using (response.Data)
    {
        using var targetStream = File.Create("downloaded_image.jpg");
        await response.Data.ContentStream.CopyToAsync(targetStream);
    }
}

```

### 2. Загрузка больших файлов чанками (Large Files API)

Пошаговый сценарий отправки составного файла, размер которого превышает стандартные лимиты одиночного запроса Backblaze:

```csharp
var fileName = "photoVideo123.mp4";

// Шаг 1: Инициализируем сессию сборки большого файла
var start = await _fileService.B2StartLargeFile(new B2StartLargeFileRequest
{
    bucketId = _options.BasketId,
    fileName = fileName,
    contentType = "video/mp4"
});

if (!start.IsSusses) throw new Exception("Не удалось начать сессию загрузки");

// Шаг 2: Нарезаем данные на части (минимум 5 000 000 байт на чанк для B2)
List<B2UploadPartRequest> chunks = SplitBase64ToChunks(base64Data, 5 * 1024 * 1024, start.Data.fileId, fileName);
var partSha1Array = new List<string>();

// Шаг 3: Последовательно отправляем все составные части
foreach (var chunk in chunks)
{
    var uploadChunk = await _fileService.B2UploadPart(chunk);
    if (uploadChunk.IsSusses)
    {
        partSha1Array.Add(uploadChunk.Data.contentSha1);
    }
}

// Шаг 4: Подаем команду на финальную сборку файла из отправленных частей
var finish = await _fileService.B2FinishLargeFile(new B2FinishLargeFileRequest
{
    fileId = start.Data.fileId,
    partSha1Array = partSha1Array
});

```

