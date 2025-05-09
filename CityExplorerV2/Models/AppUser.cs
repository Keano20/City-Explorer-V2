using System.Runtime.Serialization;using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CityExplorerV2.Models;

public class AppUser
{
    // Unique ID for each user
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    //Username of the user
    public string Username { get; set; } = String.Empty;
    
    // Hashed password of the user
    public string HashedPassword { get; set; } = String.Empty;
}