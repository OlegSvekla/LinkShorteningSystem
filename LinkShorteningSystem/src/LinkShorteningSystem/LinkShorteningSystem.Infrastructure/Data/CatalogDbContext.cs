using LinkShorteningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinkShorteningSystem.Infrastructure.Data
{
    public sealed class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {

        }
        public DbSet<Link> Links { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
    }         
}