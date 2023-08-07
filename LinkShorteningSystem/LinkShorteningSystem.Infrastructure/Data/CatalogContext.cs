using LinkShorteningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkShorteningSystem.Infrastructure.Data
{
    public sealed class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {

        }
        public DbSet<Link> Links { get; set; }
    }         
}