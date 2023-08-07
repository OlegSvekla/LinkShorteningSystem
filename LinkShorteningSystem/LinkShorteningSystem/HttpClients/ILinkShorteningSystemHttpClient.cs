namespace LinkShorteningSystem.HttpClients
{
    public interface ILinkShorteningSystemHttpClient
    {
        Task<string> CutLinkAsync(string baseLink, string origin, CancellationToken token = default);
        Task<string> GetAsync(string baseLink, string shortenedLink, CancellationToken cancellationToken = default);
    }
}