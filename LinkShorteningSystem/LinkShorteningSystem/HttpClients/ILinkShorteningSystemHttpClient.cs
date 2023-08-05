namespace LinkShorteningSystem.HttpClients;

public interface ILinkShorteningSystemHttpClient
{
    Task<string> CutLinkAsync(string baseUrl, string origin, CancellationToken token = default);
    Task<string> GetAsync(string baseUrl, string shortenedUrl, CancellationToken cancellationToken = default);
}