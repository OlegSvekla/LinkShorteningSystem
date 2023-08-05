using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using Polly.Retry;

namespace LinkShorteningSystem.HttpClients;

public class LinkShorteningSystemHttpClient : ILinkShorteningSystemHttpClient
{
    private const int RetryCount = 3;

    private readonly AsyncRetryPolicy _retryPolicy;
    private readonly HttpClient _client;

    public LinkShorteningSystemHttpClient(HttpClient client)
    {
        _client = client;
        _client.Timeout = TimeSpan.FromMinutes(1);
        _retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    public async Task<string> GetAsync(string baseUrl, string shortenedUrl, CancellationToken cancellationToken = default)
    {
        const string endpoint = "api/links/RedirectLink";

        return await _retryPolicy.ExecuteAsync(async () =>
        {
            var requestUri = $"{endpoint}?shortenedUrl={$"{baseUrl}/{shortenedUrl}"}";
            var response = await _client.GetAsync(requestUri, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync(cancellationToken);
        });
    }

    public async Task<string> CutLinkAsync(string baseUrl, string origin, CancellationToken token = default)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            var data = JsonConvert.SerializeObject(new { OriginalLink = origin, BaseUrl = baseUrl });
            var body = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/links/ShortenLink", body, token);
            response.EnsureSuccessStatusCode();
            var stringContent = await response.Content.ReadAsStringAsync(token);
            var shortened = JsonConvert.DeserializeObject<string>(stringContent)!;
            return shortened;
        });
    }
}