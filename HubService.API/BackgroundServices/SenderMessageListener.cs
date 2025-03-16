using HubService.API.Hubs;
using HubService.Application.Models;
using HubService.Application.RabbitMq.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;

namespace HubService.API.BackgroundServices
{
    public class SenderMessageListener : BackgroundService
    {
        private readonly ILogger<SenderMessageListener> _logger;
        private readonly IMessageConsumer _consumer;
        private readonly IHubContext<NotificationHub> _hubContext;

        public SenderMessageListener(ILogger<SenderMessageListener> logger, IMessageConsumer consumer, IHubContext<NotificationHub> hubContext)
        {
            _logger = logger;
            _consumer = consumer;
            _hubContext = hubContext;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.StartConsumingWithChannel<PrizeNotification>(notification =>
            {
                _logger.LogInformation("Received PrizeNotification for user {Username}, Prize {Prize}",
                    notification.CreatedBy, notification.Prize);
                var message = $"Received PrizeNotification for user {notification.CreatedBy}, Prize {notification.Prize}";
                if (NotificationHub.IsUserOnline(notification.CreatedBy))
                {
                    _hubContext.Clients.User(notification.CreatedBy)
                        .SendAsync("PrizeNotification", message);
                }
                else
                {
                    _logger.LogInformation("User {Username} is offline, notification not sent.", notification.CreatedBy);
                }
            });

            return Task.CompletedTask;
        }
    }
}
