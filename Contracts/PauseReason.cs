using System.Text.Json.Serialization;

namespace Fastclock.Contracts.Models;

[JsonConverter(typeof(JsonStringEnumConverter<PauseReason>))]
public enum PauseReason
{
    None,
    Breakfast,
    Lunch,
    Dinner,
    CoffeBreak,
    Meeting,
    DoneForToday,
    HallIsClosing,
}

public static class PauseReasonExtensions
{
    public static bool Is(this string value, PauseReason reason) =>
        reason.ToString().Equals(value, StringComparison.OrdinalIgnoreCase);
}
