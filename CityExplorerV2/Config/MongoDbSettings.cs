namespace CityExplorerV2.Config;

// Holds MongoDB connection configuration from appsettings.json
public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}