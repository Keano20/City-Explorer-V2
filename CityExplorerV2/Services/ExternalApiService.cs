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

    // Sets up the API service with access to HTTP client, config settings, and MongoDB
    public ExternalApiService(IHttpClientFactory clientFactory, IConfiguration config, MongoDbService mongoDbService)
    {
        _httpClient = clientFactory.CreateClient();
        _config = config;
        _mongoDbService = mongoDbService;
    }

    // Gets city data from the API, or returns it from cache if it has already been searched
    public async Task<string> GetCityDataAsync(string city)
    {
        var collection = _mongoDbService.CachedCities;
        var cached = await collection.Find(Builders<BsonDocument>.Filter.Eq("name", city.ToLower())).FirstOrDefaultAsync();

        if (cached != null)
            return cached["data"].AsString;

        // If not cached, make the external API call
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://wft-geo-db.p.rapidapi.com/v1/geo/cities?namePrefix={city}");
        request.Headers.Add("X-RapidAPI-Key", _config["RapidApi:Key"]);
        request.Headers.Add("X-RapidAPI-Host", "wft-geo-db.p.rapidapi.com");

        var response = await _httpClient.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();

        // Save the result to MongoDB for next time
        var doc = new BsonDocument
        {
            { "name", city.ToLower() },
            { "data", json }
        };

        await collection.InsertOneAsync(doc);
        return json;
    }

    // Gets weather info for a city, using cache if available
    public async Task<string> GetWeatherDataAsync(string city)
    {
        var collection = _mongoDbService.CachedWeather;
        var cached = await collection.Find(Builders<BsonDocument>.Filter.Eq("name", city.ToLower())).FirstOrDefaultAsync();

        if (cached != null)
            return cached["data"].AsString;

        // If not cached, call the external weather API
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://weatherapi-com.p.rapidapi.com/current.json?q={city}");
        request.Headers.Add("X-RapidAPI-Key", _config["RapidApi:Key"]);
        request.Headers.Add("X-RapidAPI-Host", "weatherapi-com.p.rapidapi.com");

        var response = await _httpClient.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();

        // Save the result to MongoDB for next time
        var doc = new BsonDocument
        {
            { "name", city.ToLower() },
            { "data", json }
        };

        await collection.InsertOneAsync(doc);
        return json;
    }
}