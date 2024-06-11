namespace AndreVeiculos.Address.Database
{
    public interface IMongoDatabaseSettings
    {
        public string AddressCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
