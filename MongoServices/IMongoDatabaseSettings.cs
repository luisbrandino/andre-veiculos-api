namespace MongoServices
{
    public interface IMongoDatabaseSettings
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string BankCollectionName { get; set; }
    }
}
