using MessageQueueServices;
using MessageQueueServices.Abstractions;
using MongoServices;
using Newtonsoft.Json;
using Repositories;

namespace AndreVeiculos.Bank.MessageProcessors
{
    public class BankInsertMessageProcessor<T> : MessageProcessor<T> where T : IMessage
    {
        private readonly BankService _service;
        private readonly IBaseRepository<Models.Bank> _repository;

        public BankInsertMessageProcessor(IConsumer<T> consumer, BankService service, IBaseRepository<Models.Bank> repository) : base(consumer)
        {
            _service = service;
            _repository = repository;
        }

        public override Task Process(T message)
        {
            var bank = JsonConvert.DeserializeObject<Models.Bank>(message.Content.ToString());

            if (bank == null)
                return Task.CompletedTask;

            _service.Insert(bank);
            _repository.Insert(bank);

            return Task.CompletedTask;
        }
    }
}
