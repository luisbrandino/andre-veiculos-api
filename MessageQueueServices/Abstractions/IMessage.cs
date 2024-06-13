namespace MessageQueueServices.Abstractions
{
    public interface IMessage
    {
        public object Content { get; set; }
    }
}
