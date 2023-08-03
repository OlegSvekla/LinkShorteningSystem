namespace LinkShorteningSystem.WebApi.Dtos
{
    public sealed class LinkDto
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortenedUrl { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
