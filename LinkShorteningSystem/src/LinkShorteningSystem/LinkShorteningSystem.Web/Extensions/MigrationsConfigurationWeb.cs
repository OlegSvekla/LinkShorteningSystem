using LinkShorteningSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LinkShorteningSystem.Web.Extensions
{
    public static class MigrationsConfigurationWeb
    {
        public static async Task<IApplicationBuilder> RunDbContextMigrationsWeb(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                var logger = serviceProvider.GetRequiredService<ILogger<IdentityDbContext>>();

                logger.LogInformation("Database migration running...");

                try
                {
                    var context = serviceProvider.GetRequiredService<IdentityDbContext>();
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
            return app;
        }
    }
}