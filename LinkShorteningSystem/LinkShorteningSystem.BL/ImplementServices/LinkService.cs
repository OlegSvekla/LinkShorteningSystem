using LinkShorteningSystem.Domain.Entities;
using LinkShorteningSystem.Domain.Exceptions;
using LinkShorteningSystem.Domain.Interfaces.Repositories;
using LinkShorteningSystem.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

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

            await _linkRepository.AddAsync(link);

            _logger.LogInformation("Link shortened: {ShortenedLink}", shortenedLink);

            return shortenedLink;
        }

        public async Task<string> GetShortenedLinkAsync(int linkId)
        {
            _logger.LogInformation("GetShortenedLinkAsync called. LinkId: {LinkId}", linkId);

            var Originallink = await _linkRepository.GetByIdAsync(linkId);
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

            var Shortlink = await _linkRepository.FirstOrDefaultAsync(l => l.ShortenedLink == shortenedLink);
            if (Shortlink is null)
            {
                throw new ShortenedLinkNotFoundException($"{shortenedLink} not found");
            }

            _logger.LogInformation("Retrieved original link: {OriginalLink}", Shortlink?.OriginalLink);

            return Shortlink?.OriginalLink;
        }

        private static string GenerateShortenedLink(string baseClientLink)
        {
            var randomChars = Guid.NewGuid().ToString("N").Substring(0, 7);
            var shortenedLink = $"{baseClientLink}/{randomChars}";
            return shortenedLink;
        }
    }
}