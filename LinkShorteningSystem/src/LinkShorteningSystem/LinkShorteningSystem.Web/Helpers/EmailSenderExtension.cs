﻿using LinkShorteningSystem.Domain.Interfaces.Services;
using System.Text.Encodings.Web;

namespace LinkShorteningSystem.Web.Helpers
{
    public static class EmailSenderExtensions
	{
		public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
		{
			return emailSender.SendEmailAsync(email, "Confirm your email",
				$"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
		}
	}
}