using System.Net.Http.Headers;
using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CityExplorerV2.Services;

public class ExternalApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly MongoDbService _mongoDbService;

    public ExternalApiService(IHttpClientFactory clientFactory, IConfiguration config, MongoDbService mongoDbService)
    {
        _httpClient = clientFactory.CreateClient();
        _config = config;
        _mongoDbService = mongoDbService;
    }

    public async Task<string> GetCityDataAsync(string city)
    {
        var collection = _mongoDbService.CachedCities;
        var cached = await collection.Find(Builders<BsonDocument>.Filter.Eq("name", city.ToLower())).FirstOrDefaultAsync();

        if (cached != null)
            return cached["data"].AsString;

        // Call RapidAPI
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://wft-geo-db.p.rapidapi.com/v1/geo/cities?namePrefix={city}");
        request.Headers.Add("X-RapidAPI-Key", _config["RapidApi:Key"]);
        request.Headers.Add("X-RapidAPI-Host", "wft-geo-db.p.rapidapi.com");

        var response = await _httpClient.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();

        // Save to MongoDB
        var doc = new BsonDocument
        {
            { "name", city.ToLower() },
            { "data", json }
        };

        await collection.InsertOneAsync(doc);
        return json;
    }

    public async Task<string> GetWeatherDataAsync(string city)
    {
        var collection = _mongoDbService.CachedWeather;
        var cached = await collection.Find(Builders<BsonDocument>.Filter.Eq("name", city.ToLower())).FirstOrDefaultAsync();

        if (cached != null)
            return cached["data"].AsString;

        // Call RapidAPI
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://weatherapi-com.p.rapidapi.com/current.json?q={city}");
        request.Headers.Add("X-RapidAPI-Key", _config["RapidApi:Key"]);
        request.Headers.Add("X-RapidAPI-Host", "weatherapi-com.p.rapidapi.com");

        var response = await _httpClient.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();

        // Save to MongoDB
        var doc = new BsonDocument
        {
            { "name", city.ToLower() },
            { "data", json }
        };

        await collection.InsertOneAsync(doc);
        return json;
    }
}