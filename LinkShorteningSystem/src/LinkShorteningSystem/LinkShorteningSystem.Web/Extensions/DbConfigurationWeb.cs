using LinkShorteningSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LinkShorteningSystem.Web.Extensions
{
    public static class DbConfigurationWeb
    {
        public static void Configuration(
            IConfiguration configuration,
            IServiceCollection services)
        {
            services.AddDbContext<IdentityDbContext>(context => 
                context.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));

            services.AddDbContext<CatalogDbContext>(context => 
                context.UseSqlServer(configuration.GetConnectionString("CatalogConnection")));
        }
    }
}
