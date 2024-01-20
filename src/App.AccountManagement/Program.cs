using App.Authentication;
using App.Core;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AuthorizeFilter("Auth"));
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
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
        _.LoginPath = "/auth/login";
        _.LogoutPath = "/auth/logout";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Auth", policy => policy.RequireAuthenticatedUser());
});

builder.Services.AddTransient<IJsonSerializer, App.Core.JsonSerializer>();
builder.Services.AddTransient<ClaimsBuilder>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
