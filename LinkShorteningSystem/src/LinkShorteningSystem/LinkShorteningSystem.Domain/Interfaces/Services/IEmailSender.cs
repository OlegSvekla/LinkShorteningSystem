namespace LinkShorteningSystem.Domain.Interfaces.Services
{
    public interface IEmailSender
	{
		Task SendEmailAsync(string email, string subject, string message);
	}
}