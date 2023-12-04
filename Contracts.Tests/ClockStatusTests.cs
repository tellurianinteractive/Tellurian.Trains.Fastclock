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
              "weekday": "Wednesday",
              "clockMode": "Fast",
              "time": "06:12:00",
              "duration": "15:00:00",
              "speed": 5.5,
              "isRunning": false,
              "isCompleted": false,
              "isElapsed": false,
              "isUnavailable": false,
              "realEndTime": "13:14:00",
              "fastEndTime": "21:00:00",
              "isPaused": false,
              "pauseReason": "Dinner",
              "pauseTime": "12:00:00",
              "expectedResumeTimeAfterPause": "13:00:00",
              "stoppedByUser": "",
              "stoppingReason": "None",
              "message": "",
              "serverVersionNumber": "4.0",
              "hostAddress": "https://fastclock.azurewebsites.net"
            }
            """;
        var json = Target.ToJson();
        //var diff = json.FirstDiff(expected); Debugger.Break();
        Assert.AreEqual(expected, json);
        var status = json.ToClockStatus();
        Assert.AreEqual(Target, status);
    }

    private static ClockStatus Target =>
    new()
    {
        Name = "Demo",
        Weekday = Weekday.Wednesday,
        Time = "06:12".ToTimeOnly(),
        Duration = TimeSpan.FromHours(15),
        Message = "",
        RealEndTime = "13:14".ToTimeOnly(),
        FastEndTime = "21:00".ToTimeOnly(),
        PauseTime = "12:00".ToTimeOnly(),
        PauseReason = PauseReason.Dinner,
        ExpectedResumeTimeAfterPause = "13:00".ToTimeOnly(),
        StoppedByUser = "",
        Speed = 5.5,
        StoppingReason = StopReason.None,
        ServerVersionNumber = "4.0",
        HostAddress = "https://fastclock.azurewebsites.net"

    };
}

