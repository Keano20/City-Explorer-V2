using CityExplorerV2.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityExplorerV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;

        public CityController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }
        /* For testing the database connection

        [HttpGet("test-connection")]
        public IActionResult TestConnection()
        {
            try
            {
                var database = _mongoDbService.GetDatabase();
                return Ok($"✅ Connected to MongoDB: {database.DatabaseNamespace.DatabaseName}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Failed to connect to MongoDB: {ex.Message}");
            }
        }
        */
    }
}