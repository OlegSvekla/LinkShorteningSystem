namespace LinkShorteningSystem.HttpClients;

public interface ILinkShorteningSystemHttpClient
{
    Task<string> CutLinkAsync(string origin, CancellationToken token = default);
    Task<string> GetAsync(string requestUri, CancellationToken cancellationToken = default);
}