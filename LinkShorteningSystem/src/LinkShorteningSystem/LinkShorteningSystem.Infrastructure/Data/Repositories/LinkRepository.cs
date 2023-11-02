using LinkShorteningSystem.Domain.Entities;
using LinkShorteningSystem.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkShorteningSystem.Infrastructure.Data.Repositories
{
    public class LinkRepository : BaseRepository<Link>, ILinkRepository
    {
        public LinkRepository(CatalogDbContext dbContext) : base(dbContext)
        {
        }
    }
}
