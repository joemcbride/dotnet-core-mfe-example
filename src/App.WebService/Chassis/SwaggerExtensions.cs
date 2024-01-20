using Microsoft.OpenApi.Models;

namespace App.WebService.Chassis;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerServices(
        this IServiceCollection services,
        bool isLocal,
        string title = "Example Jobs"
    )
    {
        var id = Guid.NewGuid();

        var description = isLocal
            ? $"<a href=\"https://localhost:5051/createtoken?id={id}&role=user\" target=\"blank\">Generate local token<a>"
            : "";

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = title,
                    Version = "v1",
                    Description = description
                }
            );
            options.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                }
            );
            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                }
            );
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerServices(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }
}
