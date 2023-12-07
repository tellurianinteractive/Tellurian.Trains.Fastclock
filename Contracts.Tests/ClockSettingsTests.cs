using Fastclock.Contracts;
using Fastclock.Contracts.Extensions;

namespace Contracts.Tests;

[TestClass]
public class ClockSettingsTests
{
    [TestMethod]
    public void SerializationAndDeseriaizationWorks()
    {
        string? expected =
            """
            {
              "name": "Demo",
              "mode": "Fast",
              "shouldReset": false,
              "isElapsed": false,
              "isRunning": false,
              "startWeekday": "None",
              "startTime": "06:00:00",
              "speed": 5.5,
              "duration": "15:00:00",
              "pauseTime": "12:00:00",
              "pauseReason": "Lunch",
              "resumeAfterPauseTime": null,
              "showRealtimeDuringPause": false,
              "overriddenElapsedTime": null,
              "message": null,
              "administratorPassword": "password",
              "userPassword": "password",
              "timeZoneId": null
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
            Duration = (15.0).AsTotalHours(),
            Mode = ClockMode.Fast,
            Name = "Demo",
            Speed = 5.5,
            UserPassword = "password",
            StartTime = new TimeOnly(6, 0),
            PauseReason = PauseReason.Lunch,
            PauseTime = new TimeOnly(12, 0)
        };
 
}