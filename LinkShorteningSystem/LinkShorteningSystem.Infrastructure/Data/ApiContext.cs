using LinkShorteningSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkShorteningSystem.Infrastructure.Data
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }

        // DbSet для работы с таблицей ссылок
        public DbSet<Link> Links { get; set; }

        // Если у вас есть другие сущности, добавьте их DbSet здесь

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Здесь вы можете добавить настройки для сущностей или изменить их конфигурацию

            modelBuilder.Entity<Link>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OriginalUrl).IsRequired();
                entity.Property(e => e.ShortenedUrl).IsRequired();
                entity.Property(e => e.CreatedDate).IsRequired();
            });

            // Если у вас есть другие настройки сущностей, добавьте их здесь

            base.OnModelCreating(modelBuilder);
        }
    }
}
