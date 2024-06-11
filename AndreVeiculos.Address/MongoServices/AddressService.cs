using AndreVeiculos.Address.Database;
using MongoDB.Driver;

namespace AndreVeiculos.Address.MongoServices
{
    public class AddressService
    {
        private readonly IMongoCollection<Models.Address> _addresses;

        public AddressService(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _addresses = database.GetCollection<Models.Address>(settings.AddressCollectionName);
        }

        public Models.Address Insert(Models.Address address) {
            _addresses.InsertOne(address);
            return address;
        }
    }
}
