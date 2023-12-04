using System.Text;

namespace Fastclock.Contracts.Extensions;

public static class StringExtensions
{
    public static bool HasValue(this string me) =>
        !string.IsNullOrWhiteSpace(me);

    public static TimeSpan? ToTimeSpanOrNull(this string value) =>
        TimeSpan.TryParse(value, out var duration) ? duration : null;

    public static TimeOnly ToTimeOnly(this string value) => 
        TimeOnly.TryParse(value, out var time) ? time : TimeOnly.MinValue;


    public static string Random(this string characters, int length)
    {
        var random = new Random();
        if (characters.Length == 0) return string.Empty;
        var text = new StringBuilder(length);
        for (var i = 0; i < length; i++)
        {
            var c = characters[random.Next(0, characters.Length - 1)];
            text.Append(c);
        }
        return text.ToString();
    }

    public static (int pos, string text) FirstDiff(this string text1, string text2)
    {
        var length = Math.Min(text1.Length, text2.Length);
        for (var i = 0; i < length; ++i)
        {
            if (text1[i] == text2[i]) continue;
            return (i, text1[..i]);
        }
        return (0, "");
    }
}
