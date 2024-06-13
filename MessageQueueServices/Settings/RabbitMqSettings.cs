using MessageQueueServices.Abstractions;

namespace MessageQueueServices.Settings
{
    public class RabbitMqSettings : IMessagingBrokerSettings
    {
        public string HostName { get; set; } = "localhost";
        public string QueueName { get; set; }
        public string? User { get; set; }
        public string? Password { get; set; }
    }
}
