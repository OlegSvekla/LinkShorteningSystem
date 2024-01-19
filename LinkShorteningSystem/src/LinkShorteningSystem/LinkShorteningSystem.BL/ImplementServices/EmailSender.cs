using LinkShorteningSystem.Domain.Interfaces.Services;

namespace LinkShorteningSystem.BL.ImplementServices
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }
    }
}
