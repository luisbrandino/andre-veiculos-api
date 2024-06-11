using Models;
using MongoDB.Driver;

namespace MongoServices
{
    public class BankService
    {
        private readonly IMongoCollection<Bank> _banks;
        public BankService(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _banks = database.GetCollection<Bank>(settings.BankCollectionName);
        }
        public void Insert(Bank bank)
        {
            if(bank != null)
                _banks.InsertOne(bank);
        }
        public Bank Find(string cnpj)
        {
            return _banks.Find(b => b.Cnpj == cnpj).FirstOrDefault();
        }
        public List<Bank> Find()
        {
            return _banks.Find(_ => true).ToList();
        }
    }
}
