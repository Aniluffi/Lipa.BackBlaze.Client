using BackBlaze.Client.IService;
using BackBlaze.Client.Options;
using BackBlaze.Client.Service;
using Http.Client.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BackBlaze.Client.Extensions
{
    public static class SturtupExstension
    {
        public static void AddFileService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient(configuration);

            services.Configure<BackBazeB2Options>(configuration.GetSection(nameof(BackBazeB2Options)));

            services.AddTransient<IBackblazeAuthService, BackblazeAuthService>();
            services.AddTransient<IFileB2Service, FileB2Service>();
        }
    }
}
