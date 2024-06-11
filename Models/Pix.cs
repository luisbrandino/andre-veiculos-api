using System.ComponentModel.DataAnnotations;

namespace Models
{
    [Table("tb_pix")]
    public class Pix : Model
    {
        [PrimaryKey]
        [AutoIncrement]
        [Key]
        public int Id { get; set; }
        [Column("pix_type_id")]
        public PixType Type { get; set; }
        public string PixKey { get; set; }
    }
}
