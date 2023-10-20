using LinkShorteningSystem.Domain.Entities;
using LinkShorteningSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Microsoft.eShopWeb.Web.Areas.Identity.Pages.Account;


public class LogoutModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<LogoutModel> _logger;

    public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<IActionResult> OnPost()
    {
        await _signInManager.SignOutAsync();
        await HttpContext.SignOutAsync();
        _logger.LogInformation("User logged out.");      
        return RedirectToPage("/Index");
    }
}