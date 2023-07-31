using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkShorteningSystem.Domain.Interfaces.Services
{
    public interface ILinkService
    {
        Task<string> ShortenLinkAsync(string originalUrl);
        Task<string> GetShortenedLinkAsync(int linkId);
        Task<string> GetOriginalLinkAsync(string shortenedUrl);
    }
}
