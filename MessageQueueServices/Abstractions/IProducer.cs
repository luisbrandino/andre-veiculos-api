namespace MessageQueueServices.Abstractions
{
    public interface IProducer<T> where T : IMessage
    {
        Task ProduceAsync(T message);
    }
}
