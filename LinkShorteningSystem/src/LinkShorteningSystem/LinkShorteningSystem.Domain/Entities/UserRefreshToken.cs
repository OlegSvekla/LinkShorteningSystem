using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkShorteningSystem.Domain.Entities
{
    public class UserRefreshToken : BaseEntity
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? RefreshToken { get; set; }
    }
}
