using MassTransit;
using Microsoft.Extensions.Logging;
using Shared;

namespace API.Consumer;

public class SendNotificationWhenAccountCreatedConsumer : IConsumer<AccountCreated>
{
    private readonly ILogger<SendNotificationWhenAccountCreatedConsumer> _logger;
    public async Task Consume(ConsumeContext<AccountCreated> context)
    {
        await Task.Delay(1);
        _logger.LogInformation("Sending notification to user {UserId} that account was created", context.Message.UserId);
    }
}
