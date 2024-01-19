namespace LinkShorteningSystem.Domain.Exceptions
{
    public sealed class OriginalLinkNotFoundException : Exception
    {
        public OriginalLinkNotFoundException(string message) : base(message)
        {
        }
    }
}