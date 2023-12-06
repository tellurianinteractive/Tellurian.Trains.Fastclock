using Fastclock.Contracts.Extensions;

namespace Fastclock.Contracts.Tests;

[TestClass]
public class ClockUserTests
{
    [TestMethod]
    public void SerializationAndDeseriaizationWorks()
    {
        var expected =
            """
            {
              "ipAddress": "127.0.0.0",
              "userName": "Stefan",
              "clientVersion": "4.0",
              "lastClockAccessTimestamp": "2023-12-05T11:56:30+01:00"
            }
            """;
        var json = Target.ToJson();
        Assert.AreEqual(expected, json);
    }

    private static ClockUser Target => new()
    {
        IPAddress ="127.0.0.0",
        UserName = "Stefan",        
        ClientVersion= "4.0",
        LastClockAccessTimestamp = new DateTimeOffset(2023,12,5,11,56,30,TimeSpan.FromHours(1)),
    };
}
