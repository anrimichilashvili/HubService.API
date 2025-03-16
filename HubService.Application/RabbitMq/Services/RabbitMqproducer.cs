using HubService.Application.RabbitMq.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace HubService.Application.RabbitMq.Services
{
    public class RabbitMqproducer : IMessageProducer
    {
        private readonly IRabbitMqConnection _connection;
        private readonly IConfiguration _configuration;
        public RabbitMqproducer(IRabbitMqConnection connection, IConfiguration configuration)
        {
            _connection = connection;
            _configuration = configuration;
        }

        public void SendMessage<T>(T message)
        {
            var exchangeName = _configuration["SenderHubServiceRabbitMq:ExchangeName"];
            var routingKey = _configuration["SenderHubServiceRabbitMq:RoutingKey"];
            var queueName = _configuration["SenderHubServiceRabbitMq:QueueName"];

            using var channel = _connection.Connection.CreateModel();

            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            channel.QueueDeclare(queueName, false, false, false, null);
            channel.QueueBind(queueName, exchangeName, routingKey, null);


            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchangeName, routingKey, false, null, body);
        }
    }
}
