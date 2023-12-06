namespace Fastclock.Contracts;

public interface IClock
{
    string Name { get; init; }
    ClockSettings Settings { get; init; }
    ClockStatus Status { get; }
    IEnumerable<ClockUser> ClockUsers { get; }

    event EventHandler<string>? OnUpdate;

    bool UpdateSettings(string? ipAddress, string? username, string? password, ClockSettings settings);
    bool UpdateUser(string? ipAddress, string? username, string clientVersion = "");
    bool IsUser(string? password, bool requirePassword = false);
    bool IsAdministrator(string? password);
    bool TryStartTick(string userName, string? password);
    bool TryStopTick(string userName, string? password, StopReason reason);
}
