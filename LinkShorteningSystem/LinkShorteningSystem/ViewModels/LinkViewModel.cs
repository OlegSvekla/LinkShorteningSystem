namespace LinkShorteningSystem.ViewModels
{
    public sealed class LinkViewModel
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortenedUrl { get; set; }
    }
}
