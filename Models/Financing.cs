using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("tb_financing")]
    public class Financing : Model
    {
        [Key]
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        [Column("sale_id")]
        public Sale Sale { get; set; }
        public DateTime FinancingDate { get; set; }
        [Column("bank_cnpj")]
        public Bank Bank { get; set; }

    }
}
