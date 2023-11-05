using LinkShorteningSystem.BL.ImplementServices;
using LinkShorteningSystem.Domain.Entities;
using LinkShorteningSystem.Domain.Interfaces.Repositories;
using LinkShorteningSystem.Domain.Interfaces.Services;
using LinkShorteningSystem.Infrastructure.Data;
using LinkShorteningSystem.Infrastructure.Data.Repositories;
using Serilog;

namespace LinkShorteningSystem.WebApi.ServicesConfigurationApi
{
    public static class ConfigurationServicesApi
    {
        internal static IServiceCollection AddCoreServices(
            this IServiceCollection services,
            IConfiguration configuration)       
        {
            services.AddControllers();
            services.AddRazorPages();
            services.AddHttpClient();
            services.AddEndpointsApiExplorer();

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ILinkRepository, LinkRepository>();
            services.AddScoped<ILinkService, LinkService>();

            return services;
        }
    }
}