using App.Authentication;
using App.Website.Chassis;
using App.Website.Schema;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var section = builder.Configuration.GetSection(nameof(GraphQLOptions));
builder.Services.Configure<GraphQLOptions>(section);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AuthorizeFilter("Auth"));
});
builder.Services.AddDomainServices();
builder.Services.AddGraphQLServices();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
});

// this can be saved to S3 or a database
var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
builder.Services
    .AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(folder))
    .SetApplicationName("SharedCookieApp");

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(_ =>
    {
        _.Cookie.Name = "SharedCookie";
        _.Cookie.Path = "/";
        _.Events.OnRedirectToLogin = context =>
        {
            var returnUrl = context.Request.GetEncodedUrl();
            context.Response.Redirect($"https://localhost:5003/auth/login?returnUrl={returnUrl}");
            return Task.CompletedTask;
        };

        _.Events.OnValidatePrincipal = async context =>
        {
            var claimsBuilder = context.HttpContext.RequestServices.GetRequiredService<IAuthenticationClaimsBuilder>();
            var principal = await claimsBuilder.BuildClaimsPrincipal(context.Principal, context.Scheme.Name);
            context.Principal = principal;
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Auth", policy => policy.RequireAuthenticatedUser());
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

var pathBase = builder.Configuration["PathBase"];
if (!string.IsNullOrWhiteSpace(pathBase))
{
    app.UsePathBase(pathBase);
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseGraphQLPlayground("/graphql/playground", new GraphQL.Server.Ui.Playground.PlaygroundOptions
    {
        GraphQLEndPoint = "/api/graphql",
        SchemaPollingInterval = 10000,
        PlaygroundSettings = new Dictionary<string, object>
        {
            {"request.credentials", "same-origin" }
        }
    });
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
