﻿using System.Globalization;

namespace Fastclock.Contracts.Extensions;

public static class TimespanExtensions
{

    public static string AsTime(this TimeSpan me)
    {
        return me.ToString(@"hh\:mm", CultureInfo.InvariantCulture);
    }

    public static string AsTimeOrEmpty(this TimeSpan? me)
    {
        if (me.HasValue && me.Value > TimeSpan.Zero) return me.Value.AsTime();
        return string.Empty;
    }

    public static TimeSpan AsTotalHours(this double hours) =>
        TimeSpan.FromHours(hours);
}
