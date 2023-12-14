using Fastclock.Contracts;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Fastclock.Server.Clocks;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class ClockServer
{
    public static Version? ServerVersion => Assembly.GetAssembly(typeof(Clock))?.GetName().Version;

    private readonly IOptions<ClockConfiguration> _options;
    private readonly ILogger<Clock> _logger;
    private readonly Dictionary<string, IClock> _clocks;

    public ClockServer(IOptions<ClockConfiguration> options, ILogger<Clock> logger)
    {
        _options = options;
        _logger = logger;
        _clocks = new Dictionary<string, IClock>
        {
            {
                "DEMO",
                new Clock(options,  logger) {
                    Name = "Demo", 
                    AdministratorPassword = "password",
                }
            }
        };
    }

    public int Count
    {
        get
        {
            lock (_clocks)
            {
                return _clocks.Count;
            }
        }
    }
    public IEnumerable<string> Names
    {
        get
        {
            lock (_clocks)
            {
                return _clocks.Select(s => s.Value.Name);
            }
        }
    }

    public  IClock? Instance(string? name)
    {
        lock (_clocks)
        {
            if (string.IsNullOrEmpty(name)) return _clocks.First().Value;
            if (_clocks.TryGetValue(name.ToUpperInvariant(), out var clock)) return clock;
            return null;
        }
    }

    public bool Create(ClockSettings settings, string userName, string? remoteIPAddress)
    {
        if (string.IsNullOrWhiteSpace(settings.Name) || string.IsNullOrWhiteSpace(settings.AdministratorPassword)) return false;
        var key = settings.Name.ToUpperInvariant();
        lock (_clocks)
        {
            if (_clocks.ContainsKey(key)) return false;
            var clock = new Clock(_options, _logger) { Name = settings.Name, AdministratorPassword = settings.AdministratorPassword };
            var isCreated = clock.UpdateSettings(remoteIPAddress, userName, settings.AdministratorPassword, settings);
            if (isCreated)
            {
                _clocks.Add(key, clock);
                _logger.LogInformation("Clock {ClockName} created.", clock.Name);
            }
            return isCreated;
        }
    }
}
