using CityExplorerV2.Config;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CityExplorerV2.Services;
public class MongoDbService
{
    private readonly IMongoDatabase _database;

    public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings)
    {
        try // A try / catch block for testing the database for connection
        {
            Console.WriteLine($"Connecting to MongoDB with: {mongoDbSettings.Value.ConnectionString}");
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            _database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            Console.WriteLine("Successfully connected to MongoDB"); 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"MongoDB connection failed: {ex.Message}");
            throw;
        }
    }
    
    public IMongoDatabase GetDatabase() => _database;
}