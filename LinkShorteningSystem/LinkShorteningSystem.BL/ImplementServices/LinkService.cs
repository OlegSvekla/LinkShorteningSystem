using LinkShorteningSystem.Domain.Entities;
using LinkShorteningSystem.Domain.Interfaces.Repositories;
using LinkShorteningSystem.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LinkShorteningSystem.BL.ImplementServices
{
    public sealed class LinkService : ILinkService
    {
        private readonly IRepository<Link> _linkRepository;
        private readonly ILogger<LinkService> _logger;

        public LinkService(IRepository<Link> linkRepository, ILogger<LinkService> logger)
        {
            _linkRepository = linkRepository;
            _logger = logger;
        }

        public async Task<string> ShortenLinkAsync(string baseClientLink, string originalLink)
        {
            _logger.LogInformation("ShortenLinkAsync called. OriginalLink: {OriginalLink}", originalLink);

            var shortenedLink = GenerateShortenedLink(baseClientLink);
            _logger.LogInformation("Generated shortened link: {ShortenedLink}", shortenedLink);

            var link = new Link
            {
                OriginalLink = originalLink,
                ShortenedLink = shortenedLink,
                CreatedDate = DateTime.Now
            };

            await _linkRepository.AddAsync(link);

            _logger.LogInformation("Link shortened: {ShortenedLink}", shortenedLink);

            return shortenedLink;
        }

        public async Task<string> GetShortenedLinkAsync(int linkId)
        {
            _logger.LogInformation("GetShortenedLinkAsync called. LinkId: {LinkId}", linkId);

            var link = await _linkRepository.GetByIdAsync(linkId);

            _logger.LogInformation("Retrieved shortened link: {ShortenedLink}", link?.ShortenedLink);

            return link?.ShortenedLink;
        }

        public async Task<string> GetOriginalLinkAsync(string shortenedLink)
        {
            _logger.LogInformation("GetOriginalLinkAsync called. ShortenedLink: {ShortenedLink}", shortenedLink);

            var link = await _linkRepository.FirstOrDefaultAsync(l => l.ShortenedLink == shortenedLink);

            _logger.LogInformation("Retrieved original link: {OriginalLink}", link?.OriginalLink);

            return link?.OriginalLink;
        }

        private static string GenerateShortenedLink(string baseClientLink)
        {
            var randomChars = Guid.NewGuid().ToString("N").Substring(0, 7);
            var shortenedLink = $"{baseClientLink}/{randomChars}";
            return shortenedLink;
        }
    }
}