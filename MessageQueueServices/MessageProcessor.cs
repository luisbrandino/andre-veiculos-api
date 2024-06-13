using MessageQueueServices.Abstractions;
using Microsoft.Extensions.Hosting;

namespace MessageQueueServices
{
    public abstract class MessageProcessor<T> : BackgroundService where T : IMessage
    {
        private readonly IConsumer<T> _consumer;

        protected MessageProcessor(IConsumer<T> consumer)
        {
            _consumer = consumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumer.ConsumeAsync(async (T message) => {
                if (stoppingToken.IsCancellationRequested)
                    return;

                await Process(message);
            });
        }

        public abstract Task Process(T message);
    }
}
