using System.Text.Json.Serialization;

namespace Fastclock.Contracts;

[JsonConverter(typeof(JsonStringEnumConverter<StopReason>))]
public enum StopReason
{
    None,
    Delays,
    DriverShortage,
    VehicleShortage,
    VehicleBreakdown,
    Derailment,
    TrackProblem,
    StationProblem,
    TechicalError,
    BoosterError,
    LocoNetError,
    Other
}

public static class StopReasonExtensions
{
    public static bool ShoundTrainsContinueRunning(this StopReason reason) => reason <= StopReason.DriverShortage;

    public static IEnumerable<string> StopReasons =>
        ((StopReason[])Enum.GetValues(typeof(StopReason))).Select(m => m.ToString());
}

