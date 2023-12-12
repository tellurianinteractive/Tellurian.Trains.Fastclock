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
    readonly ILogger<Clock> Logger = new NullLogger<Clock>();

    [TestMethod]
    public void AddsNewUser()
    {
        var target = CreateClock(ClockConfiguration);
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
        var target = CreateClock(ClockConfiguration);
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
        var target = CreateClock(ClockConfiguration);
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
        var configuration = ClockConfiguration;
        var now = CreateTimeZoneInfo(configuration.TimeZoneId).Now();
        var target = CreateClock(configuration);
        var status = target.Status;
        Assert.AreEqual("Demo", status.Name);
        Assert.AreEqual("06:00".ToTimeOnly(), status.Session.Time);
        Assert.AreEqual(15.0, status.Session.Duration.TotalHours);
        Assert.AreEqual(5.5, status.Session.Speed);
        Assert.AreEqual("21:00".ToTimeOnly(), status.Session.FastEndTime);
        Assert.AreEqual(new TimeOnly(now.Hour, now.Minute).Add(TimeSpan.FromHours(status.Session.Duration.TotalHours / status.Session.Speed)), status.Session.RealEndTime);
        Assert.AreEqual(new TimeOnly(now.Hour, now.Minute), status.Realtime.Time);
        Assert.AreEqual(now.DayOfWeek == 0 ? 7 : (int)now.DayOfWeek, (int)status.Realtime.Weekday);
        Assert.AreEqual(Weekday.Thursday, status.Session.Weekday);
    }

    [TestMethod]
    public void DemoClockAfterSettingsUpdate()
    {
        var configuration = ClockConfiguration;
        var now = CreateTimeZoneInfo(configuration.TimeZoneId).Now();
        var target = CreateClock(configuration);
        var updated = target.UpdateSettings("192.168.1.2", "Stefan", "password", ClockSettings with { Name = "Demo", AdministratorPassword = "password" });
        Assert.IsTrue(updated, "Not updated.");
        var settings = target.Settings;
        var status = target.Status;
        Assert.AreEqual("password", settings.AdministratorPassword);
        Assert.AreEqual("", settings.UserPassword);
        Assert.AreEqual("07:00".ToTimeOnly(), status.Session.Time);
        Assert.AreEqual(12.0, status.Session.Duration.TotalHours);
        Assert.AreEqual(4.0, status.Session.Speed);
        Assert.AreEqual("19:00".ToTimeOnly(), status.Session.FastEndTime);
        Assert.AreEqual(new TimeOnly(now.Hour, now.Minute).Add(TimeSpan.FromHours(status.Session.Duration.TotalHours / status.Session.Speed + 1)), status.Session.RealEndTime);
    }

    [TestMethod]
    public async Task RunDemoClockFromConfiguration()
    {
        var target = CreateClock(ClockConfiguration with { Speed = 50 });
        target.TryStartTick("Stefan", "password");
        await Task.Delay(2000);
        Assert.IsTrue(target.IsRunning);
        target.TryStopTick("Stefan", "password", StopReason.None);
        Assert.IsTrue(target.IsElapsed, "Not elapsed.");
        Assert.IsTrue(target.IsStopped, "Not stopped.");
        var status = target.Status;
        Assert.AreEqual("06:01".ToTimeOnly(), status.Session.Time);

    }

    private Clock CreateClock(ClockConfiguration configuration) => new Clock(Options.Create(configuration), Logger);

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