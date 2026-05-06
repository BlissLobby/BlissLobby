using App.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infra.Sms;

public class DummySmsSender(ILogger<DummySmsSender> logger) : ISmsSender
{
    public Task SendSmsAsync(string number, string message)
    {
        logger.LogInformation($"Sending SMS to {number} with message: {message}");
        return Task.CompletedTask;
    }
}
