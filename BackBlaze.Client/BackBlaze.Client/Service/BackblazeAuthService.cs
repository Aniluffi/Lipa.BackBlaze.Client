using BackBlaze.Client.IService;
using BackBlaze.Client.IService.Models.Response;
using BackBlaze.Client.Options;
using Http.Client.IService;
using Http.Client.Models;
using Microsoft.Extensions.Options;
using System.Text;

namespace BackBlaze.Client.Service
{
    public class BackblazeAuthService : IBackblazeAuthService
    {
        private IHttpClient _httpClient;
        private BackBazeB2Options _options;
        private BackblazeAuthState? _auth;

        public BackblazeAuthService(IOptions<BackBazeB2Options> options)
        {
            _options = options.Value;

            _httpClient = new Http.Client.Service.HttpClient(_options.AuthPatch, new Header
            {
                TypeAuth = "Basic",
                AccessToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_options.ApplicationKeyId}:{_options.ApplicationKey}")),
            });
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
