using LinkShorteningSystem.Domain.Entities;
using LinkShorteningSystem.Domain.Interfaces.Repositories;
using LinkShorteningSystem.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace LinkShorteningSystem.BL.ImplementServices
{
    public sealed class LinkService : ILinkService
    {
        private readonly ILinkRepository _linkRepository;
        private readonly ILogger<LinkService> _logger;

        public LinkService(ILinkRepository linkRepository, ILogger<LinkService> logger)
        {
            _linkRepository = linkRepository;
            _logger = logger;
        }

        public async Task<string> ShortenLinkAsync(string baseClientLink, string originalLink)
        {
            var shortenedLink = GenerateShortenedLink(baseClientLink);
            if(shortenedLink is null)
            {
                _logger.LogInformation("Unable to generate shortenLink");
               return null;
            }

            var link = new Link
            {
                OriginalLink = originalLink,
                ShortenedLink = shortenedLink,
                CreatedDate = DateTime.Now
            };

            await _linkRepository.CreateAsync(link);

            return shortenedLink;
        }

        public async Task<string> GetShortenedLinkAsync(int linkId)
        {
            var originallink = await _linkRepository.GetOneByAsync(expression: _=>_.Id.Equals(linkId));
            if (originallink is null)
            {
                return null;
            }

            return originallink?.ShortenedLink;
        }

        public async Task<string> GetOriginalLinkAsync(string shortenedLink)
        {
            var shortlink = await _linkRepository.GetOneByAsync(expression: _ => _.ShortenedLink.Equals(shortenedLink));
            if (shortlink is null)
            {
                return null;
            }

            return shortlink?.OriginalLink;
        }

        private static string GenerateShortenedLink(string baseClientLink)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(baseClientLink));
                var base64Hash = Convert.ToBase64String(hashBytes);
                var randomChars = base64Hash.Substring(0, 12);

                var shortenedLink = $"{baseClientLink}/{randomChars}";
                return shortenedLink;
            }
        }
    }
}