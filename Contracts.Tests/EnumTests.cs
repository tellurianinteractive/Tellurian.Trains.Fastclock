using static Fastclock.Contracts.Extensions.EnumExtensions;

namespace Fastclock.Contracts.Tests;

[TestClass]
public class EnumTests
{
    [TestMethod]
    public void ListsDisplayModes()
    {
        string[] expected = ["Digital", "Analogue"];
        var actual = Items<DisplayMode>();
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ListsTimeSources()
    {
        string[] expected = ["Fast", "Real"];
        var actual = Items<TimeSource>();
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ListsPauseReasons()
    {
        string[] expected = ["None", "Breakfast", "Lunch", "Dinner", "CoffeBreak", "Meeting", "DoneForToday", "HallIsClosing"];
        var actual = Items<PauseReason>();
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ListsStopReasons()
    {
        string[] expected = ["None", "Delays", "DriverShortage", "VehicleShortage", "VehicleBreakdown", "Derailment", "TrackProblem", "StationProblem", "TechicalError", "BoosterError", "LocoNetError", "Other"];
        var actual = Items<StopReason>();
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ListsWeekdays()
    {
        string[] expected = ["None", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];
        var actual = Items<Weekday>();
        CollectionAssert.AreEqual(expected, actual);
    }
}
