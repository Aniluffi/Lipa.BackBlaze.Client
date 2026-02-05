# Lipa.BackBlaze.Client

NuGet-библиотека для работы с **Backblaze B2 Cloud Storage** на `.NET 8`.

## Возможности
- Авторизация в Backblaze B2 (`b2_authorize_account`) с кэшированием токена.
- Загрузка файла в bucket (`b2_get_upload_url` + `b2_upload_file`).
- Удаление версии файла (`b2_delete_file_version`).
- Подключение через Dependency Injection (`IServiceCollection`).

## Установка
```bash
dotnet add package Lipa.BackBlaze.Client
```

## Подключение в DI
В проекте вызовите расширение:

```csharp
services.AddFileService(configuration);
```

Метод регистрирует:
- `IBackblazeAuthService`
- `IFileB2Service`

## Конфигурация
Библиотека читает секцию `BackBazeB2Options` из `appsettings.json`.

> Рекомендуется хранить ключи в Secret Manager / переменных окружения, а не в открытом json.

```json
{
  "BackBazeB2Options": {
    "ApplicationKeyId": "your-key-id",
    "ApplicationKey": "your-application-key",
    "AuthPatch": "https://api.backblazeb2.com/b2api/v4/b2_authorize_account",
    "BasketId": "your-bucket-id"
  }
}
```

## Использование

### Загрузка файла
`UploadFile` принимает base64-содержимое и имя файла.

```csharp
using BackBlaze.Client.IService;
using BackBlaze.Client.IService.Models.Request;

public class FileUploader
{
    private readonly IFileB2Service _fileB2Service;

    public FileUploader(IFileB2Service fileB2Service)
    {
        _fileB2Service = fileB2Service;
    }

    public async Task UploadAsync(string fileName, byte[] fileBytes)
    {
        var response = await _fileB2Service.UploadFile(new UploadFileRequest
        {
            fileName = fileName,
            base64 = Convert.ToBase64String(fileBytes)
        });

        if (!response.IsSusses)
            throw new Exception(response.ErrorMessage);

        Console.WriteLine($"Uploaded: {response.Data.fileId}");
    }
}
```

### Удаление файла
```csharp
var response = await _fileB2Service.DeleteFileVersion(new DeleteFileVersionRequest
{
    fileId = "file-id",
    fileName = "file-name.ext"
});

if (!response.IsSusses)
    throw new Exception(response.ErrorMessage);
```

## API интерфейсы
- `IBackblazeAuthService` — получение/обновление токена авторизации.
- `IFileB2Service` — методы загрузки и удаления файлов.

## Таргет
- `.NET 8.0`

## Лицензия
MIT
