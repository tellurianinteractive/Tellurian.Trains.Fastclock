using Fastclock.Contracts;
using Fastclock.Contracts.Extensions;
using Fastclock.Server.Clocks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using static Fastclock.Contracts.Extensions.TimeZoneInfoExtensions;

namespace Fastclock.Server.Tests;

[TestClass]
public class ClockTests
{
    readonly IOptions<ClockConfiguration> Options = Microsoft.Extensions.Options.Options.Create(ClockConfiguration);
    readonly ILogger<Clock> Logger = new NullLogger<Clock>();
    [TestMethod]
    public void AddsNewUser()
    {
        var target = new Clock(Options, Logger);
        target.UpdateUser("192.168.1.2", "Stefan", "1.2.3");
        var user = target.ClockUsers.Single(cu => cu.UserName == "Stefan");
        Assert.AreEqual("Stefan", user.UserName);
        Assert.AreEqual("192.168.1.2", user.IPAddress?.ToString());
        Assert.AreEqual("1.2.3", user.ClientVersion);
        Assert.IsTrue((DateTime.Now - user.LastClockAccessTimestamp) < TimeSpan.FromMilliseconds(200));
    }

    [TestMethod]
    public async Task UpdatesUser()
    {
        var target = new Clock(Options, Logger);
        target.UpdateUser("192.168.1.2", "Stefan", "1.2.3");
        await Task.Delay(5);
        target.UpdateUser("192.168.1.2", "Stefan", "1.2.3");
        Assert.AreEqual(1, target.ClockUsers.Count());
        var user = target.ClockUsers.Single(cu => cu.UserName == "Stefan");
        Assert.IsTrue((DateTime.Now - user.LastClockAccessTimestamp) < TimeSpan.FromMilliseconds(2));
    }

    [TestMethod]
    public async Task UpdatesUnknownUser()
    {
        var target = new Clock(Options, Logger);
        target.UpdateUser("192.168.1.2", null, "1.2.3");
        await Task.Delay(1);
        target.UpdateUser("192.168.1.2", "Stefan", "1.2.3");
        Assert.AreEqual(1, target.ClockUsers.Count());
        var user = target.ClockUsers.Single(cu => cu.UserName == "Stefan");
        Assert.IsTrue((DateTime.Now - user.LastClockAccessTimestamp) < TimeSpan.FromMilliseconds(2));
    }

    [TestMethod]
    public void ClockFromConfiguration()
    {
        var now = CreateTimeZoneInfo().Now();
        var target = new Clock(Options, Logger);
        var status = target.Status;
        Assert.AreEqual("Demo", status.Name);
        Assert.AreEqual("06:00".ToTimeOnly(), status.Time);
        Assert.AreEqual(15.0, status.Duration.TotalHours);
        Assert.AreEqual(5.5, status.Speed);
        Assert.AreEqual("21:00".ToTimeOnly(), status.FastEndTime);
        Assert.AreEqual(new TimeOnly(now.Hour, now.Minute).Add(TimeSpan.FromHours(status.Duration.TotalHours / status.Speed)), status.RealEndTime);
        Assert.AreEqual(Weekday.Thursday, status.Weekday);
    }

    [TestMethod]
    public void DemoClockAfterSettingsUpdate()
    {
        var now = CreateTimeZoneInfo().Now();
        var target = new Clock(Options, Logger);
        var updated = target.UpdateSettings("192.168.1.2", "Stefan", "password", ClockSettings with { Name="Demo", AdministratorPassword="password"});
        Assert.IsTrue(updated, "Not updated.");
        Assert.AreEqual("password", target.AdministratorPassword);
        Assert.AreEqual("", target.UserPassword);
        Assert.AreEqual("07:00".ToTimeOnly(), target.Time);
        Assert.AreEqual(12.0, target.SessionDuration.TotalHours);
        Assert.AreEqual(4.0, target.Speed);
        Assert.AreEqual("19:00".ToTimeOnly(), target.FastEndTime);
        Assert.AreEqual(new TimeOnly(now.Hour, now.Minute).Add(TimeSpan.FromHours(target.SessionDuration.TotalHours / target.Speed + 1)), target.RealEndTime);
    }

    private static ClockConfiguration ClockConfiguration { get; } = new()
    {
        Name = "Demo",
        Password = "password",
        Duration = TimeSpan.FromHours(15),
        Speed = 5.5,
        StartWeekday = Weekday.Thursday,
        StartTime = "06:00".ToTimeOnly(),
        TimeZoneId = "Central Europe Standard Time",
    };

    private static ClockSettings ClockSettings { get; } = new()
    {
        StartTime = "07:00".ToTimeOnly(),
        StartWeekday = Weekday.Monday,
        Duration = TimeSpan.FromHours(12),
        Message = "Hello!",
        PauseReason = PauseReason.Lunch,
        PauseTime = "12:00".ToTimeOnly(),
        ResumeAfterPauseTime = "13:00".ToTimeOnly(),
        Speed = 4.0,
        AdministratorPassword = "admin",
        UserPassword = "user",
    
         

    };
}