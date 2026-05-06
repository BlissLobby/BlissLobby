using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace Infra.Email;

public class DummyEmailSender(ILogger<DummyEmailSender> logger) : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        logger.LogInformation("Sending mail...");
        logger.LogInformation($"To: {email}");
        logger.LogInformation($"Subject: {subject}");
        logger.LogInformation($"Message: {htmlMessage}");
        return Task.CompletedTask;
    }
}
