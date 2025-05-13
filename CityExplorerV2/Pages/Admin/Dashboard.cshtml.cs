using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CityExplorerV2.Pages.Admin;

public class DashboardModel : PageModel
{
    public IActionResult OnGet()
    {
        var isAdmin = HttpContext.Session.GetString("IsAdmin");
        var username = HttpContext.Session.GetString("Username");

        if (string.IsNullOrEmpty(username) || isAdmin != "true")
        {
            return RedirectToPage("/Login");
        }

        return Page();
    }
}