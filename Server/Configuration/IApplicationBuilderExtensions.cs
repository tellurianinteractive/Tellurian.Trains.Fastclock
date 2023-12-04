using Fastclock.Contracts.Extensions;

namespace Fastclock.Server.Configuration;

/// <summary>
/// Extensions for building application.
/// </summary>
public static class IApplicationBuilderExtensions
{
    /// <summary>
    /// Customisation of configuration of Swagger API document and web user interface.
    /// </summary>
    public static void UseCustomisedSwagger(this IApplicationBuilder app) 
    {
        app.UseSwagger(c => c.RouteTemplate = "openapi/{documentName}/openapi.json");
        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = "openapi";
            c.SwaggerEndpoint("/openapi/v4/openapi.json", "Version 4 documentation");
            c.DocumentTitle = "Fastclock Open API";
        });
    }
    /// <summary>
    /// Customisation of localisation.
    /// </summary>
    public static void UseCustomisedLocalisation(this IApplicationBuilder app)
    {
        app.UseRequestLocalization(options =>
        {
            options.SetDefaultCulture(LanguageExtensions.DefaultLanguage);
            options.AddSupportedCultures(LanguageExtensions.SupportedTwoLetterIsoLanguages);
            options.AddSupportedUICultures(LanguageExtensions.SupportedTwoLetterIsoLanguages);
            options.FallBackToParentCultures = true;
            options.FallBackToParentUICultures = true;
        });
    }
}
