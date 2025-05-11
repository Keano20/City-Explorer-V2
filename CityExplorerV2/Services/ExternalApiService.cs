using System.Net.Http.Headers;
using System.Text.Json;

namespace CityExplorerV2.Services;

public class ExternalApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public ExternalApiService(IHttpClientFactory clientFactory, IConfiguration config)
    {
        _httpClient = clientFactory.CreateClient();
        _config = config;
    }

    public async Task<string> GetCityDataAsync(string city)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"https://wft-geo-db.p.rapidapi.com/v1/geo/cities?namePrefix={city}");

        request.Headers.Add("X-RapidAPI-Key", _config["RapidApi:Key"]);
        request.Headers.Add("X-RapidAPI-Host", "wft-geo-db.p.rapidapi.com");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync(); // or deserialize JSON here
    }

    public async Task<string> GetWeatherDataAsync(string city)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"https://weatherapi-com.p.rapidapi.com/current.json?q={city}");

        request.Headers.Add("X-RapidAPI-Key", _config["RapidApi:Key"]);
        request.Headers.Add("X-RapidAPI-Host", "weatherapi-com.p.rapidapi.com");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}