using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    [Table("bank")]
    public class Bank : Model
    {
        [BsonId]
        [Key]
        [PrimaryKey]
        public string Cnpj {  get; set; }
        public string Name { get; set; }
        public DateTime FoundationDate { get; set; }
    }
}
