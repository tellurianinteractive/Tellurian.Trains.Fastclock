namespace Fastclock.Contracts.Extensions;

public static class TimeZoneInfoExtensions
{
    public static TimeZoneInfo CreateTimeZoneInfo(string? timeZoneId = null) =>
        TimeZoneInfo.TryFindSystemTimeZoneById(timeZoneId.HasValue() ? timeZoneId : DefaultTimeZoneId, out var timeZoneInfo) ? timeZoneInfo :
        TimeZoneInfo.FindSystemTimeZoneById(DefaultTimeZoneId);

    private const string DefaultTimeZoneId = "W. Europe Standard Time";

    public static DateTime Now(this TimeZoneInfo timeZone) => DateTime.UtcNow + timeZone.GetUtcOffset(DateTimeOffset.Now);
}
