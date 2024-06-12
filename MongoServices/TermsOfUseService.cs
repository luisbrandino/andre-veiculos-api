using Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoServices
{
    public class TermsOfUseService
    {
        private readonly IMongoCollection<TermsOfUse> _termsOfUse;
        public TermsOfUseService(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _termsOfUse = database.GetCollection<TermsOfUse>(settings.TermsOfUseCollectionName);
        }

        public void Insert(TermsOfUse termsOfUse)
        {
            if (termsOfUse != null)
                _termsOfUse.InsertOne(termsOfUse);
        }

        public TermsOfUse? Find(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                return null;

            return _termsOfUse.Find(terms => terms.Id == id).FirstOrDefault();
        }
        public List<TermsOfUse> Find()
        {
            return _termsOfUse.Find(_ => true).ToList();
        }
    }
}
