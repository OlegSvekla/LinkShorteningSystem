namespace LinkShorteningSystem.Domain.Entities
{
    public sealed class Link : BaseEntity
    {
        public string OriginalLink { get; set; }
        public string ShortenedLink { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}