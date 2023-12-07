using Fastclock.Contracts.Extensions;

namespace Fastclock.Contracts.Tests;

[TestClass]
public class ClockStatusTests
{
    [TestMethod]
    public void SerializationAndDeseriaizationWorks()
    {
        var expected =
            """
            {
              "name": "Demo",
              "isUnavailable": false,
              "message": "A message to all of you!",
              "session": {
                "weekday": "Wednesday",
                "time": "06:12:00",
                "speed": 5.5,
                "duration": "15:00:00",
                "breakTime": "14:00:00",
                "realEndTime": "13:14:00",
                "fastEndTime": "21:00:00",
                "isCompleted": false,
                "isElapsed": false,
                "isRunning": false
              },
              "realtime": {
                "weekday": "Saturday",
                "time": "14:31:00"
              },
              "stopping": {
                "userName": "Stefan",
                "reason": "BoosterError"
              },
              "pause": {
                "isPaused": false,
                "reason": "Dinner",
                "time": "12:00:00",
                "sessionResumeTime": "13:00:00"
              },
              "serverVersionNumber": "4.0"
            }
            """;
        var json = WithAllPropertiesSet.ToJson();
        var diff = json.FirstDiff(expected); System.Diagnostics.Debugger.Break();
        Assert.AreEqual(expected, json);
        var status = json.ToClockStatus();
        Assert.AreEqual(WithAllPropertiesSet, status);
    }

    private static ClockStatus WithAllPropertiesSet =>
    new()
    {
        Name = "Demo",
        Message = "A message to all of you!",
        ServerVersionNumber = "4.0",
        Session = new()
        {
            Weekday = Weekday.Wednesday,
            Time = "06:12".ToTimeOnly(),
            Duration = TimeSpan.FromHours(15),
            RealEndTime = "13:14".ToTimeOnly(),
            FastEndTime = "21:00".ToTimeOnly(),
            Speed = 5.5,
            BreakTime = "14:00".ToTimeOnly(),
        },
        Realtime = new()
        {
            Time="14:31".ToTimeOnly(),
            Weekday = Weekday.Saturday,
        },
        Stopping = new()
        {
            UserName = "Stefan",
            Reason = StopReason.BoosterError,

        },
        Pause = new()
        {
            Time = "12:00".ToTimeOnly(),
            Reason = PauseReason.Dinner,
            SessionResumeTime = "13:00".ToTimeOnly(),

        },     
    };

    [TestMethod]
    public void SerializationAndDeseriaizationWithMinimumPropertiesWorks()
    {
        var expected =
            """
            {
              "name": "Demo",
              "isUnavailable": false,
              "message": null,
              "session": {
                "weekday": "None",
                "time": "06:12:00",
                "speed": 5.5,
                "duration": "15:00:00",
                "breakTime": null,
                "realEndTime": "13:14:00",
                "fastEndTime": "21:00:00",
                "isCompleted": false,
                "isElapsed": false,
                "isRunning": false
              },
              "realtime": {
                "weekday": "Saturday",
                "time": "14:31:00"
              },
              "stopping": null,
              "pause": null,
              "serverVersionNumber": "4.0"
            }
            """;
        var json = WithMinimumPropertiesSet.ToJson();
        var diff = json.FirstDiff(expected); System.Diagnostics.Debugger.Break();
        Assert.AreEqual(expected, json);
        var status = json.ToClockStatus();
        Assert.AreEqual(WithMinimumPropertiesSet, status);
    }
    private static ClockStatus WithMinimumPropertiesSet =>
    new()
    {
        Name = "Demo",
        Message = null,
        ServerVersionNumber = "4.0",
        Session = new()
        {
            Time = "06:12".ToTimeOnly(),
            Duration = TimeSpan.FromHours(15),
            RealEndTime = "13:14".ToTimeOnly(),
            FastEndTime = "21:00".ToTimeOnly(),
            Speed = 5.5,

        },
        Realtime = new()
        {
            Time = "14:31".ToTimeOnly(),
            Weekday = Weekday.Saturday,
        },
        Stopping = null,
        Pause = null,
        
    };
}

