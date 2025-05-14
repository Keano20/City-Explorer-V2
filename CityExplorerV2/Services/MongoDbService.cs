using CityExplorerV2.Config;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CityExplorerV2.Services;
public class MongoDbService
{
    private readonly IMongoDatabase _database;
    
    // Connects to MongoDB using the connection details from appsettings.json
    public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings)
    {
        try
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            _database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            
            // Sends a simple ping to check if the database is reachable
            _database.RunCommand<BsonDocument>("{ping:1}");
            Console.WriteLine($"Successfully connected to database: {mongoDbSettings.Value.DatabaseName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"MongoDB connection failed: {ex.Message}");
            throw;
        }
    }
    // Returns the main database connection so other parts of the app can use it
    public IMongoDatabase GetDatabase() => _database;
    
    // Holds cached city search results to reduce repeated external API calls
    public IMongoCollection<BsonDocument> CachedCities =>
        _database.GetCollection<BsonDocument>("CachedCities");
    
    // Holds cached weather results for previously searched cities
    public IMongoCollection<BsonDocument> CachedWeather =>
        _database.GetCollection<BsonDocument>("CachedWeather");
}