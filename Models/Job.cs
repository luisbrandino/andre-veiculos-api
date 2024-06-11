namespace Models
{
    [Table("tb_job")]
    public class Job : Model
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Description { get; set; }

    }
}
