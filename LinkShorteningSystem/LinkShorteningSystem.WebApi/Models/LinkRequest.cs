using System.Text.Json.Serialization;

namespace LinkShorteningSystem.WebApi.Models;

public class LinkRequest
{
    public string OriginalLink { get; set; }
}