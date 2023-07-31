using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkShorteningSystem.Domain.Entities
{
    public sealed class Link
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortenedUrl { get; set; }
        public DateTime CreatedDate { get; set; }

        // Дополнительные поля, если требуется связь с пользователем
        //public int? UserId { get; set; }
        //public User User { get; set; }
    }
}
