using Fastclock.Server.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSwagger();

var app = builder.Build();

var isHttpsDisabled = app.Configuration.GetValue("DisableHttps", false);

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseDeveloperExceptionPage();
}
else
{
    if(!isHttpsDisabled) app.UseHsts();
}
app.UseCustomisedSwagger();
app.UseCustomisedLocalisation();

if (!isHttpsDisabled) app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Logger.LogInformation("App version {AppVersion} starting at {AppStartTime}", Assembly.GetExecutingAssembly().GetName().Version, DateTimeOffset.Now.ToString("g"));
if (isHttpsDisabled) app.Logger.LogWarning("HTTPS and HSTS are disabled.");

app.Run();
