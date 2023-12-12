using System.Text.Json.Serialization;

namespace Fastclock.Contracts;

[JsonConverter(typeof(JsonStringEnumConverter<DisplayMode>))]

public enum DisplayMode
{
    Digital,
    Analogue
}
