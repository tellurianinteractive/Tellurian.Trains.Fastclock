using Fastclock.Contracts;
using Fastclock.Contracts.Extensions;
using Fastclock.Contracts.Models;

namespace Contracts.Tests;

[TestClass]
public class ClockSettingsTests
{
    [TestMethod]
    public void SerializationAndDeseriaizationWorks()
    {
        var expected =
            """
            {
              "name": "Demo",
              "shouldRestart": false,
              "isElapsed": false,
              "isRunning": false,
              "startWeekday": "0",
              "startTime": "06:00:00",
              "speed": 5.5,
              "durationHours": "15:00:00",
              "pauseTime": "12:00:00",
              "pauseReason": "Lunch",
              "expectedResumeTime": null,
              "showRealTimeWhenPaused": false,
              "overriddenElapsedTime": null,
              "message": "",
              "mode": "Fast",
              "administratorPassword": "password",
              "userPassword": "password"
            }
            """;
        var json = Target.ToJson();
        //var diff = json.FirstDiff(expected); Debugger.Break();
        Assert.AreEqual(expected, json, "Serialisation failed.");
        var setting = json.ToClockSettings();
        Assert.AreEqual(Target, setting);
    }

    private static ClockSettings Target =>
        new()
        {
            AdministratorPassword = "password",
            DurationHours = (15.0).AsTotalHours(),
            Mode = ClockMode.Fast,
            Name = "Demo",
            Speed = 5.5,
            UserPassword = "password",
            StartTime = new TimeOnly(6, 0),
            StartWeekday = "0",
            PauseReason = PauseReason.Lunch,
            Message = "",
            PauseTime = new TimeOnly(12, 0)
        };
 
}