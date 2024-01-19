namespace LinkShorteningSystem.Domain.Interfaces.Services
{
    public interface ILinkService
    {
        Task<string> ShortenLinkAsync(string baseClientLink, string originalLink);
        Task<string> GetShortenedLinkAsync(int linkId);
        Task<string> GetOriginalLinkAsync(string shortenedLink);
    }
}