using Fastclock.Server.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSwagger();

var app = builder.Build();
var httpsDisabled = app.Configuration.GetValue("DisableHttps", false);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseDeveloperExceptionPage();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    if(!httpsDisabled) app.UseHsts();
}
app.UseCustomisedSwagger();
app.UseCustomisedLocalisation();

if (!httpsDisabled) app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Logger.LogInformation("App version {version} starting at {time}", Assembly.GetExecutingAssembly().GetName().Version, DateTimeOffset.Now.ToString("g"));

app.Run();
