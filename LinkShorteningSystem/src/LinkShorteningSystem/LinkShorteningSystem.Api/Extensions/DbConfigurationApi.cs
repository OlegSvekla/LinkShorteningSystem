using LinkShorteningSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LinkShorteningSystem.Web.Extensions
{
    public static class DbConfigurationApi
    {
        public static void Configuration(
            IConfiguration configuration,
            IServiceCollection services)
        {
            services.AddDbContext<CatalogDbContext>(context => 
                context.UseSqlServer(configuration.GetConnectionString("CatalogConnection")));

        }
    }
}
