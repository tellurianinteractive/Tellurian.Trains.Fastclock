using System.Text.Json;

namespace Fastclock.Contracts.Extensions;
public static class JsonSerializationExtensions
{
    /// <summary>
    /// These options are used for all serialization and deserialization in the fastclock API.
    /// </summary>
    public static JsonSerializerOptions Options { get; } = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = false,
        PreferredObjectCreationHandling = System.Text.Json.Serialization.JsonObjectCreationHandling.Populate,
    };



    public static string ToJson(this ClockSettings settings) => JsonSerializer.Serialize(settings, Options);
    public static ClockSettings? ToClockSettings(this Json json) => JsonSerializer.Deserialize<ClockSettings>(json, Options);

    public static string ToJson(this ClockStatus status) => JsonSerializer.Serialize(status, Options);
    public static ClockStatus? ToClockStatus(this Json json) => JsonSerializer.Deserialize<ClockStatus>(json, Options);

    public static string ToJson(this ClockUser user) => JsonSerializer.Serialize(user, Options);
    public static ClockUser? ToClockUser(this Json json) => JsonSerializer.Deserialize<ClockUser>(json, Options);
}
