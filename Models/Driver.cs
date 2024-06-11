using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("tb_driver")]
    public class Driver : Person
    {
        [Column("license_id")]
        public DriverLicense License { get; set; }  
    }
}
