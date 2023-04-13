using App.Website.Chassis;
using App.Website.Schema;

var builder = WebApplication.CreateBuilder(args);

var section = builder.Configuration.GetSection(nameof(GraphQLOptions));
builder.Services.Configure<GraphQLOptions>(section);

builder.Services.AddControllersWithViews();
builder.Services.AddDomainServices();
builder.Services.AddGraphQLServices();

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
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

app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseGraphQLPlayground("/graphql/playground", new GraphQL.Server.Ui.Playground.PlaygroundOptions
    {
        GraphQLEndPoint = "/api/graphql",
        SchemaPollingInterval = 10000
    });
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
