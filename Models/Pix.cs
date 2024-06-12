using System.ComponentModel.DataAnnotations;

namespace Models
{
    [Table("pix")]
    public class Pix : Model
    {
        [PrimaryKey]
        [AutoIncrement]
        [Key]
        public int Id { get; set; }
        [Column("type_id")]
        public PixType Type { get; set; }
        public string PixKey { get; set; }
    }
}
