using CityExplorerV2.Models;
using CityExplorerV2.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CityExplorerV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;
        private readonly IMongoCollection<City> _cities;

        // Constructor: Initializes the City collection from MongoDB
        public CityController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
            _cities = _mongoDbService.GetDatabase().GetCollection<City>("Cities");
        }

        // POST: api/city
        // Adds a new city to the database
        [HttpPost]
        public async Task<IActionResult> CreateCity([FromBody] City newCity)
        {
            if (string.IsNullOrWhiteSpace(newCity.Name) ||
                string.IsNullOrWhiteSpace(newCity.Country) ||
                string.IsNullOrWhiteSpace(newCity.Region))
            {
                return BadRequest("City name, country, and region are required.");
            }

            await _cities.InsertOneAsync(newCity);
            return CreatedAtAction(nameof(GetCityById), new { id = newCity.Id }, newCity);
        }
        
        // GET: api/city
        // Returns all cities in the collection
        [HttpGet]
        public async Task<IActionResult> GetAllCities()
        {
            var cities = await _cities.Find(_ => true).ToListAsync();
            return Ok(cities);
        }

        // GET: api/city/{id}
        //Returns a single city by its ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCityById(string id)
        {
            var city = await _cities.Find(city => city.Id == id).FirstOrDefaultAsync();
            if (city == null) return NotFound();
            return Ok(city);
        }
        
        // PUT: api/city/{id}
        // Updates an existing city by its ID
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCity(string id, [FromBody] City updateCity)
        {
            updateCity.Id = id;
            var result = await _cities.ReplaceOneAsync(city => city.Id == id, updateCity);
            
            if (result.MatchedCount == 0)
                return NotFound();
            
            return NoContent();
        }

        // DELETE: api/city/{id}
        // Deletes a user by their ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(string id)
        {
            var result = await _cities.DeleteOneAsync(city => city.Id == id);
            
            if (result.DeletedCount == 0)
                return NotFound();
            
            return NoContent();
        }
    }
}