using BackBlaze.Client.IService;
using BackBlaze.Client.IService.Models.Response;
using BackBlaze.Client.Options;
using Http.Client.IService;
using Http.Client.Models;
using Http.Client.Options;
using Microsoft.Extensions.Options;
using System.Text;

namespace BackBlaze.Client.Service
{
    public class BackblazeAuthService : IBackblazeAuthService
    {
        private readonly IHttpClient _httpClient;
        private readonly BackBazeB2Options _options;
        private BackblazeAuthState? _auth;
        private readonly HttpClientOptions _httpClientOptions;

        public BackblazeAuthService(IOptions<BackBazeB2Options> options,IOptions<HttpClientOptions> optionsHttp)
        {
            _options = options.Value;
            _httpClientOptions = optionsHttp.Value;
            _httpClient = new Http.Client.Service.HttpClient(_options.AuthPatch, new Header
            {
                TypeAuth = "Basic",
                AccessToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_options.ApplicationKeyId}:{_options.ApplicationKey}")),
            },_httpClientOptions);
        }

        public async Task<BackblazeAuthState> GetAuthAsync()
        {
            if (_auth != null && _auth.expiresAtUtc > DateTime.UtcNow)
                return _auth;

            var response = await _httpClient.SendAsync<BackblazeAuthState, object?>("", HttpMethod.Get, null);

            _auth = response.Data;
            _auth.expiresAtUtc = DateTime.UtcNow.AddHours(20);

            Console.WriteLine("токен авторизации BackBlaze b2 обновлен");

            return _auth;
        }
    }
}
