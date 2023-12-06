using Fastclock.Contracts.Extensions;
using Fastclock.Server.Clocks;
using System.Text.Json;
using static Fastclock.Contracts.Extensions.JsonSerializationExtensions;

namespace Fastclock.Server.Tests;

[TestClass]
public class AppSettingsExample
{
    [TestMethod]
    public void CreateAppsettingsExample()
    {
        var target = new AppSettings();
        File.WriteAllText("ExampleAppsettings.json", JsonSerializer.Serialize(target, Options));
    }

    public class AppSettings
    {
        public ClockConfiguration ClockConfiguration { get; set; } = new()
        {
            Name = "Demo",
            Password = "password",
            Duration = TimeSpan.FromHours(15),
            Speed = 5.5,
            StartTime = "06:00".ToTimeOnly(),
        };
    }

   
}
