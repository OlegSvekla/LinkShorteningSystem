using LinkShorteningSystem.Domain.Entities;
using LinkShorteningSystem.Domain.Interfaces.Repositories;

namespace LinkShorteningSystem.Infrastructure.Data.Repositories
{
    public class LinkRepository : BaseRepository<Link>, ILinkRepository
    {
        public LinkRepository(CatalogDbContext dbContext) : base(dbContext)
        {
        }
    }
}