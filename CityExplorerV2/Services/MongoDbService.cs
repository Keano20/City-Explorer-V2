using CityExplorerV2.Config;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CityExplorerV2.Services;
public class MongoDbService
{
    private readonly IMongoDatabase _database;

    public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings)
    {
        try // A try / catch block for testing the database for connection
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            _database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            
            // Forces MongoDB to respond by issuing a ping
            _database.RunCommand<BsonDocument>("{ping:1}");
            Console.WriteLine($"Successfully connected to database: {mongoDbSettings.Value.DatabaseName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"MongoDB connection failed: {ex.Message}");
            throw;
        }
    }
    
    public IMongoDatabase GetDatabase() => _database;
}