using MessageQueueServices.Abstractions;

namespace MessageQueueServices.Messages
{
    public class RabbitMqMessage : IMessage
    {
        public object Content { get; set; }
    }
}
