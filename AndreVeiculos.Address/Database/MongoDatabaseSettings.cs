namespace AndreVeiculos.Address.Database
{
    public class MongoDatabaseSettings : IMongoDatabaseSettings
    {
        public string AddressCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
