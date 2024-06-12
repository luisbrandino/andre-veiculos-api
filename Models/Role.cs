using System.ComponentModel.DataAnnotations;

namespace Models
{
    [Table("role")]
    public class Role : Model
    {
        [PrimaryKey]
        [AutoIncrement]
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
