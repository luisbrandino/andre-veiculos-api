namespace Models
{
    [Table("tb_employee")]
    public class Employee : Person
    {
        [Column("role_id")]
        public Role Role { get; set; }
        public decimal Commission { get; set; }
    }
}
