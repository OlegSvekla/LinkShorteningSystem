namespace LinkShorteningSystem.HttpClients;

public interface ILinkShorteningSystemHttpClient
{
    Task<string> CutLinkAsync(string origin, CancellationToken token = default);
}