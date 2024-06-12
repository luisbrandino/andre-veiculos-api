using System.ComponentModel.DataAnnotations;

namespace Models
{
    [Table("category")]
    public class Category : Model
    {
        [PrimaryKey]
        [Key]
        [AutoIncrement]
        public int Id {  get; set; }
        public string Description { get; set; }
    }
}
