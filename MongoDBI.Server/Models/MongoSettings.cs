namespace MongoDBI.Server.Models
{
    public class MongoSettings
    {
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";

        public string DatabaseName { get; set; } = "yoloNiklas";

        public string CollectionName { get; set; } = "agendo";
    }
}
