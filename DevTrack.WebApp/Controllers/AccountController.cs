using System.Security.Claims;
using DevTrack.Shared.Security;
using DevTrack.WebApp.Auth;
using DevTrack.WebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DevTrack.WebApp.Controllers;

public class AccountController : Controller
{
    private readonly IAdminCredentialValidator _credentialValidator;
    private readonly IAdminJwtTokenService _jwtTokenService;
    private readonly AdminAuthOptions _adminOptions;

    public AccountController(
        IAdminCredentialValidator credentialValidator,
        IAdminJwtTokenService jwtTokenService,
        IOptions<AdminAuthOptions> adminOptions)
    {
        _credentialValidator = credentialValidator;
        _jwtTokenService = jwtTokenService;
        _adminOptions = adminOptions.Value;
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectAfterLogin(returnUrl);
        }

        return View(new LoginViewModel { ReturnUrl = returnUrl ?? Url.Content("~/") });
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (!_credentialValidator.Validate(model.Username, model.Password))
        {
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View(model);
        }

        var displayName = _adminOptions.DisplayName;
        var accessToken = _jwtTokenService.CreateAdminToken(model.Username, displayName);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, model.Username),
            new(ClaimTypes.Name, displayName),
            new(ClaimTypes.Role, AuthConstants.AdminRole)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
            IsPersistent = false
        };
        authProperties.StoreTokens(new[]
        {
            new AuthenticationToken { Name = "access_token", Value = accessToken }
        });

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

        return RedirectAfterLogin(model.ReturnUrl);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Denied()
    {
        return View();
    }

    private IActionResult RedirectAfterLogin(string? returnUrl)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }
}
