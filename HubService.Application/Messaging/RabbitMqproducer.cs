using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace HubService.Application.Messaging
{
    public class RabbitMqproducer : IMessageProducer
    {
        private readonly IRabbitMqConnection _connection;
        public RabbitMqproducer(IRabbitMqConnection connection)
        {
            _connection = connection;
        }

        public void SendMessage<T>(T message)
        {
            string exchangeName = "HubServiceExchange";
            string routingKey = "HubService-routing-key";
            string queueName = "HubService";

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
