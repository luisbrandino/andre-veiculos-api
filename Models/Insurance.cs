using System.ComponentModel.DataAnnotations;

namespace Models
{
    [Table("tb_insurance")]
    public class Insurance : Model
    {
        [PrimaryKey]
        [Key]
        [AutoIncrement]
        public int Id { get; set; }

        [Column("client_identification_number")]
        public Client Client { get; set; }

        [Column("car_license_plate")]
        public Car Car {  get; set; }

        [Column("driver_identification_number")]
        public Driver Driver { get; set; }

        public decimal Deductible { get; set; }

    }
}
