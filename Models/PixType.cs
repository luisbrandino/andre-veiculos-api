namespace Models
{
    [Table("tb_pix_type")]
    public class PixType : Model
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
