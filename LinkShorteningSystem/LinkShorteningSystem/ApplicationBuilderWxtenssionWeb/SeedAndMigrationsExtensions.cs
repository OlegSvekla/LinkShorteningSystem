using LinkShorteningSystem.Infrastructure.Data;
using LinkShorteningSystem.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace LinkShorteningSystem.WebApi.ApplicationBuilderExtension
{
    public static class SeedAndMigrationsExtensionsWeb
    {
        public static IApplicationBuilder AppExtensionWeb(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var scopedProvider = scope.ServiceProvider;
                try
                {
                    var identityContext = scopedProvider.GetRequiredService<AppIdentityDbContext>();
                    if (identityContext.Database.IsSqlServer())
                    {
                        identityContext.Database.Migrate();
                    }
                }
                catch (Exception ex)
                {
                    var logger = scopedProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while adding migrations to the Database");
                }
            }

            return app;
        }
    }
}
