using LinkShorteningSystem.Domain.Entities;
using LinkShorteningSystem.Domain.Interfaces.Repositories;
using LinkShorteningSystem.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LinkShorteningSystem.BL.ImplementServices
{
    public sealed class LinkService : ILinkService
    {
        private readonly IRepository<Link> _linkRepository;

        public LinkService(IRepository<Link> linkRepository)
        {
            _linkRepository = linkRepository;
        }

        public async Task<string> ShortenLinkAsync(string baseClientUrl, string originalUrl)
        {
            var shortenedUrl = GenerateShortenedUrl(baseClientUrl);

            var link = new Link
            {
                OriginalUrl = originalUrl,
                ShortenedUrl = shortenedUrl,
                CreatedDate = DateTime.Now
            };

            await _linkRepository.AddAsync(link);

            return shortenedUrl;
        }

        public async Task<string> GetShortenedLinkAsync(int linkId)
        {
            var link = await _linkRepository.GetByIdAsync(linkId);

            return link?.ShortenedUrl;
        }

        public async Task<string> GetOriginalLinkAsync(string shortenedUrl)
        {
            var link = await _linkRepository.FirstOrDefaultAsync(l => l.ShortenedUrl == shortenedUrl);

            return link?.OriginalUrl;
        }

        private static string GenerateShortenedUrl(string baseClientUrl)
        {          
            var randomChars = Guid.NewGuid().ToString("N").Substring(0, 7); 
            var shortenedUrl = $"{baseClientUrl}/{randomChars}";
            return shortenedUrl;
        }
    }
}