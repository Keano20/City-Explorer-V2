using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using CityExplorerV2.Models;
using CityExplorerV2.Services;

namespace CityExplorerV2.Pages;

public class LoginModel : PageModel
{
    private readonly IMongoCollection<AppUser> _users;

    public LoginModel(MongoDbService mongoDbService)
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
        var user = await _users.Find(user => user.Username == Username).FirstOrDefaultAsync();

        if (user == null || !BCrypt.Net.BCrypt.Verify(Password, user.HashedPassword))
        {
            Message = "Invalid username or password.";
            return Page();
        }
        
        // set session values
        HttpContext.Session.SetString("username", user.Username);
        HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString().ToLower());
        
        // Redirect to the admin dashboard
        return RedirectToPage("/Admin?Dashboard");
    }
}