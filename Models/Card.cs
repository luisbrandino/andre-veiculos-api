using System.ComponentModel.DataAnnotations;

namespace Models
{
    [Table("card")]
    public class Card : Model
    {
        [PrimaryKey]
        [Key]
        public string Number { get; set; }
        public string Name { get; set; }
        public string VerificationCode { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
