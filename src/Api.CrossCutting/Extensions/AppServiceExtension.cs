using Api.Domain.Interfaces;
using Api.Domain.Settings;
using Api.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.CrossCutting.Extensions
{
    public static class AppServiceExtension
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            var settingsConfig = configuration.GetSection(nameof(TwilioSettings)).Get<TwilioSettings>();

            services.AddSingleton(settingsConfig)
                    .AddTransient<IVideoService, VideoService>()
                    .AddSignalR();

            return services;
        }
    }
}
