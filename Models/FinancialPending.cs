using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("tb_financial_pending")]
    public class FinancialPending : Model
    {
        [PrimaryKey]
        [Key]
        [AutoIncrement]
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public DateTime PendingDate { get; set; }
        public DateTime BillingDate {  get; set; }
        public bool Status {  get; set; }
        [Column("client_identification_number")]
        public Client Client { get; set; }
    }
}
