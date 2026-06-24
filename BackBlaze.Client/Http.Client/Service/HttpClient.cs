using Http.Client.Common;
using Http.Client.IService;
using Http.Client.Models;
using Http.Client.Options;
using Microsoft.Extensions.Options;
using System.Text;

namespace Http.Client.Service
{
    public class HttpClient : IHttpClient
    {
        private readonly System.Net.Http.HttpClient _httpClient;

        private readonly string _basePatch;

        private readonly Header _header;

        private readonly HttpClientOptions _options;

        public HttpClient(string basePatch, Header header,HttpClientOptions options)
        {
            _options = options;
            _httpClient = new System.Net.Http.HttpClient()
            {
                Timeout = new TimeSpan(0,0,0,0,_options.TimeoutMs)
            };
            _header = header;
            _basePatch = basePatch;
        }

        public async Task<BaseResponse<TResponse>> SendAsync<TResponse, TRequest>(
     string method,
     HttpMethod httpMethod,
     TRequest request)
        {
            for (var attempt = 1; attempt <= _options.RetryCount; attempt++)
            {
                try
                {
                    using var httpRequest =
                        new HttpRequestMessage(httpMethod, _basePatch + method);

                    if (!string.IsNullOrWhiteSpace(_header.TypeAuth))
                    {
                        httpRequest.Headers.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue(
                                _header.TypeAuth,
                                _header.AccessToken);
                    }
                    else
                    {
                        httpRequest.Headers.TryAddWithoutValidation(
                            "Authorization",
                            _header.AccessToken);
                    }

                    if (request is not null)
                    {
                        var json = System.Text.Json.JsonSerializer.Serialize(request);

                        httpRequest.Content = new StringContent(
                            json,
                            Encoding.UTF8,
                            "application/json");
                    }

                    var httpResponse = await _httpClient.SendAsync(httpRequest);

                    var content = await httpResponse.Content.ReadAsStringAsync();

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        return new BaseResponse<TResponse>
                        {
                            Data = System.Text.Json.JsonSerializer.Deserialize<TResponse>(
                                string.IsNullOrWhiteSpace(content)
                                    ? "{}"
                                    : content)
                        };
                    }

                    if ((int)httpResponse.StatusCode >= 500
                        && attempt < _options.RetryCount)
                    {
                        await Task.Delay(GetDelay(attempt));
                        continue;
                    }

                    return new BaseResponse<TResponse>
                    {
                        ErrorMessage =
                            $"HTTP {(int)httpResponse.StatusCode} " +
                            $"({httpResponse.ReasonPhrase}): {content}"
                    };
                }
                catch (TaskCanceledException ex)
                {
                    // timeout

                    if (attempt < _options.RetryCount)
                    {
                        await Task.Delay(GetDelay(attempt));
                        continue;
                    }

                    return new BaseResponse<TResponse>
                    {
                        ErrorMessage = $"Timeout: {ex.Message}"
                    };
                }
                catch (HttpRequestException ex)
                {
                    // network errors

                    if (attempt < _options.RetryCount)
                    {
                        await Task.Delay(GetDelay(attempt));
                        continue;
                    }

                    return new BaseResponse<TResponse>
                    {
                        ErrorMessage = ex.Message
                    };
                }
                catch (Exception ex)
                {
                    return new BaseResponse<TResponse>
                    {
                        ErrorMessage = ex.Message
                    };
                }
            }

            return new BaseResponse<TResponse>
            {
                ErrorMessage = "Request failed"
            };
        }

        private TimeSpan GetDelay(int attempt)
        {
            return TimeSpan.FromMilliseconds(
                _options.RetryDelayMs * Math.Pow(2, attempt - 1));
        }
    }
}
