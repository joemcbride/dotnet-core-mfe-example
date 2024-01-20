using App.Infrastructure;
using App.WebService.Chassis;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false;
});

builder.Services.AddDomainServices();
builder.Services.AddHealthChecks();
builder.Services.AddProblemDetailsServices();

var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
builder.Services.AddAuthenticationServices(jwtOptions);
builder.Services.AddAuthorizationServices();

builder.Services.AddSwaggerServices(builder.Environment.IsDevelopment());

builder.Services.AddControllers(_ =>
{
    _.Filters.Add(new AuthorizeFilter(PolicyNames.Authenticated));
});

var app = builder.Build();

app.UseProblemDetails();
app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseTokenGeneratorMiddleware(jwtOptions);
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
