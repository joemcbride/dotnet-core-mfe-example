using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace App.Website.Home;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAntiforgery _antiforgery;

    public HomeController(
        ILogger<HomeController> logger,
        IAntiforgery antiforgery)
    {
        _logger = logger;
        this._antiforgery = antiforgery;
    }

    public IActionResult Index()
    {
        var token = _antiforgery.GetAndStoreTokens(HttpContext);

        var viewData = new HomeViewData
        {
            CsrfToken = token.RequestToken
        };

        return View("Index", Serialize(viewData));
    }

    private string Serialize<T>(T item) where T : class
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var json = JsonSerializer.Serialize(item, options);

        return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
    }
}

public class HomeViewData
{
    public string CsrfToken { get; set; }
}
