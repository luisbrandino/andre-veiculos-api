using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Models
{
    public class TermsOfUseAcceptance : Model
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public Client Client { get; set; }

        public TermsOfUse TermsOfUse { get; set; }

        public DateTime AcceptedAt { get; set; }
    }
}
