using System.Security.Claims;
using Carsties.Shared.Extensions.Logger;
using IdentityModel;
using IdentityService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityService.Pages.Account.Register;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly ILogger _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public Index(ILogger logger, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    [BindProperty]
    public RegisterViewModel Input { get; set; }
    [BindProperty]
    public bool RegistrationSuccess { get; set; }

    public IActionResult OnGet(string returnUrl)
    {
        Input = new RegisterViewModel
        {
            ReturnUrl = returnUrl
        };
        _logger.Here().Information("Rendering registration page. {@returnurl}", returnUrl);
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (Input.Button != "register")
        {
            _logger.Here().Information("Registration cancelled.");
            return Redirect("~/");
        }
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = Input.Username,
                Email = Input.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                await _userManager.AddClaimsAsync(user, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, Input.FullName)
                });
                RegistrationSuccess = true;
                _logger.Here().Information("{email} user created successfully", user.Email);
            }
        }
        return Page();
    }
}
