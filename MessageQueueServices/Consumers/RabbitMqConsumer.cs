using MessageQueueServices.Abstractions;
using MessageQueueServices.Messages;
using MessageQueueServices.Settings;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MessageQueueServices.Consumers
{
    public class RabbitMqConsumer : IConsumer<RabbitMqMessage>
    {
        private readonly RabbitMqSettings _settings;
        private readonly ConnectionFactory _factory;

        public RabbitMqConsumer(RabbitMqSettings settings)
        {
            _settings = settings;

            _factory = new ConnectionFactory()
            {
                HostName = settings.HostName
            };
        }

        public async Task ConsumeAsync(Func<RabbitMqMessage, Task> onReceive)
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(
              queue: _settings.QueueName,
              durable: false,
              exclusive: false,
              autoDelete: false,
              arguments: null
            );

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var data = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject<RabbitMqMessage>(data);

                if (message == null)
                    return;

                await onReceive(message);
            };

            channel.BasicConsume(
                queue: _settings.QueueName,
                autoAck: true,
                consumer: consumer
            );

            await Task.Delay(Timeout.Infinite);
        }

    }
}
