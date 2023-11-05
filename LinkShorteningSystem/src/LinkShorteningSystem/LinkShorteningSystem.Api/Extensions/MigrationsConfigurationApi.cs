using LinkShorteningSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LinkShorteningSystem.Api.Extensions
{
    public static class SeedAndMigrationsExtensionsApi
    {
        public static async Task<IApplicationBuilder> RunDbContextMigrationsApi(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var logger = serviceProvider.GetRequiredService<ILogger<CatalogDbContext>>();

                try
                {
                    var catalogContext = serviceProvider.GetRequiredService<CatalogDbContext>();
                    catalogContext.Database.Migrate();
                    
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while adding migrations to Database");
                }
            }

            return app;
        }
    }
}
