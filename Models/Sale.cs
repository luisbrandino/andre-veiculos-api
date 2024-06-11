using System.ComponentModel.DataAnnotations;

namespace Models
{
    [Table("tb_sale")]
    public class Sale : Model
    {
        [PrimaryKey]
        [AutoIncrement]
        [Key]
        public int Id { get; set; }

        [Column("car_license_plate")]
        public Car Car { get; set; }

        [Column("client_id")]
        public Client Client { get; set; }

        [Column("employee_id")]
        public Employee Employee { get; set; }

        [Column("payment_id")]
        public Payment Payment { get; set; }

        public decimal Price { get; set; }
        public DateTime SoldAt { get; set; }
    }
}
