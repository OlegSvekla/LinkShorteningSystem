using LinkShorteningSystem.BL.ImplementServices;
using LinkShorteningSystem.Domain.Entities;
using LinkShorteningSystem.Domain.Interfaces.Repositories;
using LinkShorteningSystem.Domain.Interfaces.Services;
using LinkShorteningSystem.Infrastructure.Data;
using Serilog;

namespace LinkShorteningSystem
{
    public static class ConfigurationServicesApi
    {
        internal static IServiceCollection AddCoreServices(
            this IServiceCollection services,
            IConfiguration configuration,
            ILoggingBuilder logging)
        {
            logging.ClearProviders();
            logging.AddSerilog(
                new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger());

            services.AddControllers();
            services.AddRazorPages();
            services.AddHttpClient();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<ILinkService, LinkService>();

            return services;
        }
    }
}