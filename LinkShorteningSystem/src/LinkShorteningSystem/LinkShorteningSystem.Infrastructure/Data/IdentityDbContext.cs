using Microsoft.EntityFrameworkCore;

namespace LinkShorteningSystem.Infrastructure.Data
{
    public sealed class IdentityDbContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }
    }
}