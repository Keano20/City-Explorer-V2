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
    }
}