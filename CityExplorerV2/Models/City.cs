using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CityExplorerV2.Models;

public class City
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
}