using System.Text.Json.Serialization;

namespace Fastclock.Contracts;

[JsonConverter(typeof(JsonStringEnumConverter<Weekday>))]
public enum Weekday
{
    None,
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}

public static class WeekdayExtensions {
    public static IEnumerable<string> Weekdays =>
            ((Weekday[])Enum.GetValues(typeof(Weekday))).Select(m => m.ToString());
}
