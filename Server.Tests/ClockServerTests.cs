using Fastclock.Server.Clocks;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Fastclock.Server.Tests;

[TestClass]
public class ClockServerTests
{
    [TestMethod]
    public void DefaultInstanceContainsDemoClock()
    {
        var target = new ClockServer(Options.Create(Configuration("")),  NullLogger<Clock>.Instance);
        Assert.AreEqual(1, target.Count);
        var clock = target.Instance(null);
        Assert.AreEqual("Demo", clock!.Name);
    }

    [TestMethod]
    public void AddsNewClock()
    {
        var target = new ClockServer(Options.Create(Configuration("")),  NullLogger<Clock>.Instance);
        target.Create(new() { Name = "New", AdministratorPassword="password" }, "Stefan", "127.0.0.0");
        Assert.AreEqual(2, target.Count);
        var clock = target.Instance("New");
        Assert.IsNotNull(clock);
        Assert.AreEqual("New", clock.Name);
    }

    private static ClockConfiguration Configuration(string name) => new() { Name = name, Password = "password" };
}
