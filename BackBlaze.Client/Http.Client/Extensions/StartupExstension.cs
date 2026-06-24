using Http.Client.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Http.Client.Extensions
{
    public static class StartupExstension
    {
        public static void AddHttpClient(this IServiceCollection serviceCollection,IConfiguration configuration)
        {
            serviceCollection.Configure<HttpClientOptions>(configuration.GetSection(nameof(HttpClientOptions)));
        }
    }
}
