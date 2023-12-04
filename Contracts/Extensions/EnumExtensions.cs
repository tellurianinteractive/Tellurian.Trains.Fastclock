namespace Fastclock.Contracts.Extensions;

public static class EnumExtensions
{
    public static string[] Items<T>() where T : struct =>
        ((T[])Enum.GetValues(typeof(T))).Select(m => m.ToString() ?? "").ToArray();
}
