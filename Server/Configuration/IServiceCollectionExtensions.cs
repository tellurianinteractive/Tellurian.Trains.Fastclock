using Microsoft.OpenApi.Models;

namespace Fastclock.Server.Configuration;

/// <summary>
/// Customisation of service registrations.
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Customisation of Swagger service.
    /// </summary>
    public static IServiceCollection AddSwagger(this IServiceCollection services) =>
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v4", new OpenApiInfo
            {
                Version = "v4",
                Title = "Fastclock API",
                Description = "API for getting data and control the Fastclock.",
                Contact = new OpenApiContact { Name = "Stefan Fjällemark" },
                License = new OpenApiLicense { Name = "GPL-3.0 Licence" }
            });
            c.IgnoreObsoleteProperties();
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Fastclock.Server.xml"), includeControllerXmlComments: true);
            c.EnableAnnotations();
        });

}
