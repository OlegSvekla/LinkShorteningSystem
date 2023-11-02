using LinkShorteningSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LinkShorteningSystem.WebApi.ApplicationBuilderExtensionApi
{
    public static class SeedAndMigrationsExtensionsApi
    {
        public static IApplicationBuilder AppExtenssionApi(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var scopedProvider = scope.ServiceProvider;

                try
                {
                    var catalogContext = scopedProvider.GetRequiredService<CatalogDbContext>();
                    if (catalogContext.Database.IsSqlServer())
                    {
                        catalogContext.Database.Migrate();
                    }
                }
                catch (Exception ex)
                {
                    var logger = scopedProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while adding migrations to Database");
                }
            }

            return app;
        }
    }
}
