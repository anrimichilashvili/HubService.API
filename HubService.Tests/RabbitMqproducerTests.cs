using HubService.Application.RabbitMq.Interfaces;
using HubService.Application.RabbitMq.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using RabbitMQ.Client;
using System.Text;

namespace HubService.Tests
{
    public class RabbitMqproducerTests
    {
        [Fact]
        public void SendMessage_ShouldPublishMessageToExchange()
        {
            var mockConnection = new Mock<IRabbitMqConnection>();
            var mockModel = new Mock<IModel>();

            mockConnection.Setup(c => c.Connection.CreateModel())
                          .Returns(mockModel.Object);

            var inMemorySettings = new Dictionary<string, string>
            {
                { "SenderHubServiceRabbitMq:ExchangeName", "TestExchange" },
                { "SenderHubServiceRabbitMq:RoutingKey", "TestRoutingKey" },
                { "SenderHubServiceRabbitMq:QueueName", "TestQueue" }
            };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            IMessageProducer producer = new RabbitMqproducer(mockConnection.Object, configuration);
            var sampleMessage = new { Text = "Hello, RabbitMQ!" };

            producer.SendMessage(sampleMessage);

            mockModel.Verify(m => m.ExchangeDeclare("TestExchange", ExchangeType.Direct, false, false, null), Times.Once);

            mockModel.Verify(m => m.QueueDeclare("TestQueue", false, false, false, null), Times.Once);

            mockModel.Verify(m => m.QueueBind("TestQueue", "TestExchange", "TestRoutingKey", null), Times.Once);

            mockModel.Verify(m => m.BasicPublish("TestExchange", "TestRoutingKey", false, null,
                It.Is<byte[]>(body => Encoding.UTF8.GetString(body).Contains("Hello, RabbitMQ!"))
            ), Times.Once);
        }
    }
}