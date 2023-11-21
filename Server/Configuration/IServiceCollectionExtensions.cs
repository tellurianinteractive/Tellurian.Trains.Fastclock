using Microsoft.OpenApi.Models;

namespace Fastclock.Server.Configuration;

public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Customisation of Swagger service registration.
    /// </summary>
    public static IServiceCollection AddSwagger(this IServiceCollection services) =>
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v3", new OpenApiInfo
            {
                Version = "v3",
                Title = "Module Meeting App API",
                Description = "API for getting and control the Fast Clock.",
                Contact = new OpenApiContact { Name = "Stefan Fjällemark" },
                License = new OpenApiLicense { Name = "GPL-3.0 Licence" }
            });
            c.IgnoreObsoleteProperties();
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Tellurian.Trains.MeetingApp.Server.xml"), includeControllerXmlComments: true);
            c.EnableAnnotations();
        });

}
