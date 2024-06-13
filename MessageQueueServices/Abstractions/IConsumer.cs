namespace MessageQueueServices.Abstractions
{
    public interface IConsumer<T> where T : IMessage
    {
        Task ConsumeAsync(Func<T, Task> onReceive);
    }
}
