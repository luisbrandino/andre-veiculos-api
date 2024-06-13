using MessageQueueServices.Abstractions;
using Microsoft.Extensions.Hosting;

namespace MessageQueueServices
{
    public abstract class MessageProcessor : BackgroundService
    {
        private readonly IConsumer _consumer;

        protected MessageProcessor(IConsumer consumer)
        {
            _consumer = consumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumer.ConsumeAsync(async (IMessage message) => {
                if (stoppingToken.IsCancellationRequested)
                    return;

                await Process(message);
            });
        }

        public abstract Task Process(IMessage message);
    }
}
