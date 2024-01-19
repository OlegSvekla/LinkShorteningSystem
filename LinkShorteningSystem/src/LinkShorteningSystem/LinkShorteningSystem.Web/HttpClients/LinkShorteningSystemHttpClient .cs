using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System.Text;

namespace LinkShorteningSystem.HttpClients;

public class LinkShorteningSystemHttpClient : ILinkShorteningSystemHttpClient
{
    private const int RetryCount = 3;

    private readonly AsyncRetryPolicy _retryPolicy;
    private readonly HttpClient _client;
    private readonly ILogger<LinkShorteningSystemHttpClient> _logger;

    public LinkShorteningSystemHttpClient(HttpClient client, ILogger<LinkShorteningSystemHttpClient> logger)
    {
        _client = client;
        _client.Timeout = TimeSpan.FromMinutes(1);
        _logger = logger;

        _retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning(exception, "Request failed, retrying...");
                });
    }

    public async Task<string> GetOriginalByShortendLinkAsync(string baseLink, string shortenedLink, CancellationToken cancellationToken = default)
    {
        const string endpoint = "api/links/RedirectLink";

        return await _retryPolicy.ExecuteAsync(async () =>
        {
            var requestLink = $"{endpoint}?shortenedLink={$"{baseLink}/{shortenedLink}"}";
            var response = await _client.GetAsync(requestLink, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync(cancellationToken);
        });
    }

    public async Task<string> CutLinkAsync(string baseLink, string origin, CancellationToken token = default)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            var data = JsonConvert.SerializeObject(new { OriginalLink = origin, BaseLink = baseLink });
            var body = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/links/ShortenLink", body, token);
            response.EnsureSuccessStatusCode();
            var stringContent = await response.Content.ReadAsStringAsync(token);
            var shortenedLink = JsonConvert.DeserializeObject<string>(stringContent)!;
            return shortenedLink;
        });
    }
}