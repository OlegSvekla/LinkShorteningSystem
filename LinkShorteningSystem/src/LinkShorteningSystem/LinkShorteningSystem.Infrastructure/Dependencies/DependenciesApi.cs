using LinkShorteningSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LinkShorteningSystem.Infrastructure.Dependencies
{
    public static class DependenciesApi
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddDbContext<CatalogDbContext>(context => context.UseSqlServer(configuration.GetConnectionString("CatalogConnection")));
        }
    }
}