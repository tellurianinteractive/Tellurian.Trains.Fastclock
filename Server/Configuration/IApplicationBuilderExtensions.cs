using Fastclock.Contracts.Extensions;

namespace Fastclock.Server.Configuration;

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
            c.SwaggerEndpoint("/openapi/v3/openapi.json", "Version 3 documentation");
            c.DocumentTitle = "Tellurian Trains Module Meeting App Open API";
        });
    }

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
