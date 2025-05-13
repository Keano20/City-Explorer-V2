using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using CityExplorerV2.Models;
using CityExplorerV2.Services;
using BCrypt.Net;

namespace CityExplorerV2.Pages;

public class RegisterModel : PageModel
{
    private readonly IMongoCollection<AppUser> _users;

    public RegisterModel(MongoDbService mongoDbService)
    {
        var database = mongoDbService.GetDatabase();
        _users = database.GetCollection<AppUser>("Users");
    }

    [BindProperty]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public async Task<IActionResult> OnPostAsync()
    {
        var existing = await _users.Find(user => user.Username == Username).FirstOrDefaultAsync();
        if (existing != null)
        {
            Message = "Username already exists.";
            return Page();
        }

        var hashed = BCrypt.Net.BCrypt.HashPassword(Password);
        var newUser = new AppUser
        {
            Username = Username,
            HashedPassword = hashed,
            IsAdmin = false
        };
        await _users.InsertOneAsync(newUser);

        Console.WriteLine("User registered successfully.");
        return RedirectToPage("Login");
    }
}