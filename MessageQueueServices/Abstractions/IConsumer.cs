namespace MessageQueueServices.Abstractions
{
    public interface IConsumer
    {
        Task ConsumeAsync(Func<IMessage, Task> onReceive);
    }
}
