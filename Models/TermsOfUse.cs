using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Models
{
    public class TermsOfUse : Model
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Text { get; set; }

        public int Version { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool Status { get; set; }    
    }
}
