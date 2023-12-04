namespace Fastclock.Contracts;

public class ClockUser
{
    public string IPAddress { get; set; } = string.Empty;
    public string? UserName { get; set; } = string.Empty;
    public string? ClientVersion { get; set; } = string.Empty;
    public TimeOnly LastUsedTime { get; set; }
}
