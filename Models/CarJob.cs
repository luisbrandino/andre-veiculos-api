using System.ComponentModel.DataAnnotations;

namespace Models
{
    [Table("tb_car_job")]
    public class CarJob : Model
    {
        [PrimaryKey]
        [AutoIncrement]
        [Key]
        public int Id { get; set; }

        [Column("car_license_plate")]
        public Car Car { get; set; }

        [Column("job_id")]
        public Job Job { get; set; }

        public bool Status { get; set; }
    }
}
