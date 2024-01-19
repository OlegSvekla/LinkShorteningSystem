namespace LinkShorteningSystem.Domain.Exceptions
{
    public sealed class ShortenedLinkNotFoundException : Exception
    {
        public ShortenedLinkNotFoundException(string message) : base(message)
        {
        }
    }
}