using LinkShorteningSystem.BL.ImplementServices;
using LinkShorteningSystem.Domain.Entities;
using LinkShorteningSystem.Domain.Interfaces.Repositories;
using LinkShorteningSystem.Domain.Interfaces.Services;
using LinkShorteningSystem.Infrastructure.Data;

namespace LinkShorteningSystem
{
    public static class ConfigurationServicesApi
    {
        internal static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
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