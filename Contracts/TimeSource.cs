using System.Text.Json.Serialization;

namespace Fastclock.Contracts;

[JsonConverter(typeof(JsonStringEnumConverter<TimeSource>))]
public enum TimeSource
{
    Fast,
    Real
}

public static class ClockModeExtensions
{
    public static bool IsReal(this TimeSource mode) => mode == TimeSource.Real;
    public static bool IsFast(this TimeSource mode) => mode == TimeSource.Fast;

    
}
