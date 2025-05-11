using Microsoft.AspNetCore.Mvc;
using CityExplorerV2.Services;

namespace CityExplorerV2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CityApiController : ControllerBase
{
    private readonly ExternalApiService _apiService;

    public CityApiController(ExternalApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpGet("city")]
    public async Task<IActionResult> GetCityData(string name)
    {
        var data = await _apiService.GetCityDataAsync(name);
        return Content(data, "application/json");
    }

    [HttpGet("weather")]
    public async Task<IActionResult> GetWeatherData(string city)
    {
        var data = await _apiService.GetWeatherDataAsync(city);
        return Content(data, "application/json");
    }
}