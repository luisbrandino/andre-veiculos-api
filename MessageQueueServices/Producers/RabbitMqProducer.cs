using MessageQueueServices.Abstractions;
using MessageQueueServices.Messages;
using MessageQueueServices.Settings;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace MessageQueueServices.Producers
{
    public class RabbitMqProducer : IProducer
    {
        private readonly RabbitMqSettings _settings;
        private readonly ConnectionFactory _factory;

        public RabbitMqProducer(RabbitMqSettings settings)
        {
            _settings = settings;
            _factory = new ConnectionFactory()
            {
                HostName = _settings.HostName
            };
        }

        public async Task ProduceAsync(IMessage message)
        {
            message = (RabbitMqMessage) message;

            if (message == null)
                return;

            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: _settings.QueueName,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    var stringfied = JsonConvert.SerializeObject(message);
                    var data = Encoding.UTF8.GetBytes(stringfied);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: _settings.QueueName,
                        basicProperties: null,
                        body: data
                    );
                }
            }

        }
    }
}
