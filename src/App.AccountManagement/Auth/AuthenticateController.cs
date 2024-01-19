using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace App.AccountManagement.Auth;

[Route("auth")]
public class AuthenticateController : Controller
{
    [HttpGet("login")]
    [AllowAnonymous]
    public IActionResult GetLogin(string returnUrl = null)
    {
        return View("Login", new { returnUrl = UrlEncoder.Default.Encode(returnUrl) });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> PostLogin(string returnUrl = null)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "John Doe"),
            new Claim(ClaimTypes.Role, "User"),
        };

        var claimsIdentity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        var now = DateTimeOffset.UtcNow;

        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = now.AddMinutes(10),
            // The time at which the authentication ticket expires. A 
            // value set here overrides the ExpireTimeSpan option of 
            // CookieAuthenticationOptions set with AddCookie.

            IsPersistent = true,
            // Whether the authentication session is persisted across 
            // multiple requests. When used with cookies, controls
            // whether the cookie's lifetime is absolute (matching the
            // lifetime of the authentication ticket) or session-based.

            IssuedUtc = now,

            //RedirectUri = <string>
            // The full path or absolute URI to be used as an http 
            // redirect response value.
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return Redirect(returnUrl ?? "/");
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index", "Home");
    }
}
