
namespace Fastclock.Server.Clocks;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class ClockConfiguration
{
    public required string Name { get; set; }
    public required string Password { get; set; } 
    public TimeOnly StartTime { get; set; }
    public TimeSpan Duration { get; set; }
    public double Speed { get; set; }
}

