using HubService.Application.RabbitMq.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;


namespace HubService.Application.RabbitMq.Services
{
    public class RabbitMqConnection : IRabbitMqConnection, IDisposable
    {
        private IConnection? _connection;
        public IConnection Connection => _connection;
        public RabbitMqConnection(IConfiguration configuration)
        {
            InitializeConnection(configuration);
        }

        private void InitializeConnection(IConfiguration configuration)
        {
            var rabbitMqUri = configuration.GetConnectionString("RabbitMQ");
            if (string.IsNullOrWhiteSpace(rabbitMqUri))
            {
                throw new ArgumentException("RabbitMQ connection string is not configured.");
            }

            var factory = new ConnectionFactory
            {
                Uri = new Uri(rabbitMqUri),
            };

            _connection = factory.CreateConnection();
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}
