using LinkShorteningSystem.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkShorteningSystem.BL.ImplementServices
{
    public class EmailSender : IEmailSender
    {
        // This class is used by the application to send email for account confirmation and password reset.
        // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }
    }
}
