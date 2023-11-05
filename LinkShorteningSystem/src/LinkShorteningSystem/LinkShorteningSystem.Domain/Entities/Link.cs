using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkShorteningSystem.Domain.Entities
{
    public sealed class Link : BaseEntity
    {
        public string OriginalLink { get; set; }
        public string ShortenedLink { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}