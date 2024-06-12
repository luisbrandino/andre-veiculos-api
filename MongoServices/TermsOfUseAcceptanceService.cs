using Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
namespace MongoServices
{
    public class TermsOfUseAcceptanceService
    {
        private readonly IMongoCollection<TermsOfUseAcceptance> _termsOfUseAcceptances;
        public TermsOfUseAcceptanceService(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _termsOfUseAcceptances = database.GetCollection<TermsOfUseAcceptance>(settings.TermsOfUseAcceptanceCollectionName);
        }
        public void Insert(TermsOfUseAcceptance termsOfUseAcceptance)
        {
            if (termsOfUseAcceptance != null)
                _termsOfUseAcceptances.InsertOne(termsOfUseAcceptance);
        }
        public TermsOfUseAcceptance? Find(string id)
        {
            return _termsOfUseAcceptances.Find(terms => terms.Id == id).FirstOrDefault();
        }
        public List<TermsOfUseAcceptance> Find()
        {
            return _termsOfUseAcceptances.Find(_ => true).ToList();
        }
    }
}
