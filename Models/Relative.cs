using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("relative")]
    public class Relative : Person
    {
        [Column("client_identification_number")]
        public Client Client { get; set; }
    }
}
