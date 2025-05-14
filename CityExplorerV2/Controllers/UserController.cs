using CityExplorerV2.Models;
using CityExplorerV2.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using BCrypt.Net;

namespace CityExplorerV2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase {
    private readonly IMongoCollection<AppUser> _users;

    public UserController(MongoDbService mongoDbService)
    {
        var database = mongoDbService.GetDatabase();
        _users = database.GetCollection<AppUser>("Users");
    }

    // POST: api/user/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AppUser newUser)
    {
        // Check if username already exists
        var existingUser = await _users.Find(user => user.Username == newUser.Username).FirstOrDefaultAsync();
        if (existingUser != null)
        {
            return Conflict("Username already exists.");
        }

        // Hash the password
        newUser.HashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.HashedPassword);

        await _users.InsertOneAsync(newUser);

        return Ok("User registered successfully.");
    }

    // Gets all users within the database
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _users.Find(_ => true).ToListAsync();
        return Ok(users);
    }

    // Deletes a user by ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var result = await _users.DeleteOneAsync(user => user.Id == id);
        if (result.DeletedCount == 0) return NotFound("User not found.");
        return NoContent();
    }
}