namespace LinkShorteningSystem.WebApi.Models;

public class LinkRequest
{
    public string OriginalLink { get; set; }
    public string BaseUrl { get; set; }
}