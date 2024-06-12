namespace Models
{
    [Table("client")]
    public class Client : Person
    {
        public decimal Income { get; set; }
        public string IncomeStatementFile { get; set; }
    }
}
