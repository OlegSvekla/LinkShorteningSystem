using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkShorteningSystem.Domain.Exceptions
{
    public sealed class ShortenedLinkNotFoundException : Exception
    {
        public ShortenedLinkNotFoundException(string message) : base(message)
        {
        }
    }
}