using MessageQueueServices;
using MessageQueueServices.Abstractions;
using MongoServices;
using Newtonsoft.Json;
using Repositories;

namespace AndreVeiculos.Bank.MessageProcessors
{
    public class BankInsertMessageProcessor : MessageProcessor
    {
        private readonly BankService _service;
        private readonly IBaseRepository<Models.Bank> _repository;

        public BankInsertMessageProcessor(IConsumer consumer, BankService service, IBaseRepository<Models.Bank> repository) : base(consumer)
        {
            _service = service;
            _repository = repository;
        }

        public override async Task Process(IMessage message)
        {
            var bank = JsonConvert.DeserializeObject<Models.Bank>(message.Content.ToString());

            if (bank == null)
                return;

            await Task.WhenAll(_repository.Insert(bank), _service.InsertAsync(bank));
        }
    }
}
