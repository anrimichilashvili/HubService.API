using HubService.Application.Models;
using HubService.Application.RabbitMq.Interfaces;

namespace HubService.API.BackgroundServices
{
    public class SenderMessageListener : BackgroundService
    {
        private readonly ILogger<SenderMessageListener> _logger;
        private readonly IMessageConsumer _consumer;

        public SenderMessageListener(ILogger<SenderMessageListener> logger, IMessageConsumer consumer)
        {
            _logger = logger;
            _consumer = consumer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.StartConsumingWithChannel<PrizeNotification>(message =>
            {
                _logger.LogInformation("Received sender message: {Message}", message);
            });
            return Task.CompletedTask;
        }
    }
}
