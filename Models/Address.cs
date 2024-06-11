namespace Models
{
    [Table("tb_address")]
    public class Address : Model
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Street { get; set; }
        public string StreetType {  get; set; } 
        public string PostalCode { get; set; }
        public string Neighborhood { get; set; }
        public int Number { get; set; }
        public string Complement {  get; set; }
        public string State {  get; set; }
        public string City { get; set; }
    }
}
