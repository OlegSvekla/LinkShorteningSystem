using LinkShorteningSystem.Domain.Entities;
using LinkShorteningSystem.Domain.Exceptions;
using LinkShorteningSystem.Domain.Interfaces.Repositories;
using LinkShorteningSystem.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Linq.Expressions;
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
            _logger.LogInformation("ShortenLinkAsync called. OriginalLink: {OriginalLink}", originalLink);

            var shortenedLink = GenerateShortenedLink(baseClientLink);
            if(shortenedLink is null)
            {
                throw new ShortenedLinkNotFoundException($"No shortened link were found");
            }

            _logger.LogInformation("Generated shortened link: {ShortenedLink}", shortenedLink);

            var link = new Link
            {
                OriginalLink = originalLink,
                ShortenedLink = shortenedLink,
                CreatedDate = DateTime.Now
            };

            await _linkRepository.CreateAsync(link);

            _logger.LogInformation("Link shortened: {ShortenedLink}", shortenedLink);

            return shortenedLink;
        }

        public async Task<string> GetShortenedLinkAsync(int linkId)
        {
            _logger.LogInformation("GetShortenedLinkAsync called. LinkId: {LinkId}", linkId);

            var Originallink = await _linkRepository.GetOneByAsync(expression: _=>_.Id.Equals(linkId));
            if (Originallink is null)
            {
                throw new OriginalLinkNotFoundException($"No original link were found by its id like: {linkId}");
            }

            _logger.LogInformation("Retrieved shortened link: {ShortenedLink}", Originallink?.ShortenedLink);

            return Originallink?.ShortenedLink;
        }

        public async Task<string> GetOriginalLinkAsync(string shortenedLink)
        {
            _logger.LogInformation("GetOriginalLinkAsync called. ShortenedLink: {ShortenedLink}", shortenedLink);

            var Shortlink = await _linkRepository.GetOneByAsync(expression: _=> _.ShortenedLink.Equals(shortenedLink));
            if (Shortlink is null)
            {
                throw new ShortenedLinkNotFoundException($"{shortenedLink} not found");
            }

            _logger.LogInformation("Retrieved original link: {OriginalLink}", Shortlink?.OriginalLink);

            return Shortlink?.OriginalLink;
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