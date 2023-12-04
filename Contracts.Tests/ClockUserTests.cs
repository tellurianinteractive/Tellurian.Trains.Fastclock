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
              "lastUsedTime": "12:23:00"
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
        LastUsedTime = "12:23".ToTimeOnly(),
    };
}
