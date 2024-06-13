namespace MessageQueueServices.Abstractions
{
    public interface IProducer
    {
        Task ProduceAsync(IMessage message);
    }
}
