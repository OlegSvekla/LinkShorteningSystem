using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkShorteningSystem.Domain.Exceptions
{
    public sealed class OriginalLinkNotFoundException : Exception
    {
        public OriginalLinkNotFoundException(string message) : base(message)
        {
        }
    }
}