using LinkShorteningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkShorteningSystem.Infrastructure.Data
{
    public sealed class CatalogContextSeed
    {
        public static async Task SeedAsync(CatalogContext catalogContext, ILogger logger, int retry = 0)
        {
            var retryForAvailability = retry;

            logger.LogInformation("Data seeding started.");

            try
            {
                if (!await catalogContext.Links.AnyAsync())
                {
                    await catalogContext.Links.AddRangeAsync(GetPreconfiguredLinks());
                    await catalogContext.SaveChangesAsync();
                }

                logger.LogInformation("Data seeding completed successfully.");
            }
            catch (Exception ex)
            {
                if (retryForAvailability >= 10)
                {
                    throw;
                }

                retryForAvailability++;
                logger.LogError(ex.Message);
                await SeedAsync(catalogContext, logger, retryForAvailability);
            }
        }

        private static IEnumerable<Link> GetPreconfiguredLinks()
        {
            // Здесь генерируем случайные сокращенные ссылки на основе полных ссылок
            var links = new List<Link>
        {
            new Link
            {
                OriginalUrl = "https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-7.0",
                ShortenedUrl = GenerateShortenedUrl(),
                CreatedDate = DateTime.Now.AddDays(-10)
            },
            new Link
            {
                OriginalUrl = "https://learn.microsoft.com/en-us/training/modules/branch-merge-git",
                ShortenedUrl = GenerateShortenedUrl(),
                CreatedDate = DateTime.Now.AddDays(-5)
            },
            new Link
            {
                OriginalUrl = 
                "https://learn.microsoft.com/ru-ru/aspnet/core/security/authentication/accconfirm?view=aspnetcore-7.0&tabs=visual-studio",
                ShortenedUrl = GenerateShortenedUrl(),
                CreatedDate = DateTime.Now.AddDays(-7)
            }
        };

            return links;
        }

        private static string GenerateShortenedUrl()
        {
            // Генерируем случайный набор символов для сокращенной ссылки, например, с помощью GUID
            return Guid.NewGuid().ToString("N").Substring(0, 7);
        }
    }

}
