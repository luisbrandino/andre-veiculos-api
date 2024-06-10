namespace AndreVeiculos.Address.PostalCodeServices
{
    public interface IAddressResult
    {
        public string Street { get; set; }
        public string StreetType { get; set; }
        public string PostalCode { get; set; }
        public string Neighborhood { get; set; }
        public string State { get; set; }
        public string City { get; set; }
    }
}
