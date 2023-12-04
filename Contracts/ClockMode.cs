using System.Text.Json.Serialization;

namespace Fastclock.Contracts;

[JsonConverter(typeof(JsonStringEnumConverter<ClockMode>))]
public enum ClockMode
{
    Fast,
    Real
}

public static class ClockModeExtensions
{
    public static bool IsReal(this ClockMode mode) => mode == ClockMode.Real;
    public static bool IsFast(this ClockMode mode) => mode == ClockMode.Fast;

    
}
