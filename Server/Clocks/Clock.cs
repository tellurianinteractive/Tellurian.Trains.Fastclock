using Fastclock.Contracts;
using Fastclock.Contracts.Extensions;
using Microsoft.Extensions.Options;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Fastclock.Server.Clocks;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class Clock : IDisposable, IClock
{
    private readonly ClockConfiguration _configuration;
    private readonly ILogger<Clock> _logger;
    private readonly Timer _clockTimer;
    private readonly List<ClockUser> _clients = [];

    public Clock(IOptions<ClockConfiguration> options, ILogger<Clock> logger)
    {
        _configuration = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;
        Name = _configuration.Name;
        AdministratorPassword = _configuration.Password;
        UserPassword = string.Empty;
        _clockTimer = new Timer(1000);
        _clockTimer.Elapsed += Tick;
    }


    public event EventHandler<string>? OnUpdate;
    public string Name { get; init; }
    public ClockSettings Settings { get; init; } = new ClockSettings();
    public ClockStatus Status => throw new NotImplementedException();
    public IEnumerable<ClockUser> ClockUsers => throw new NotImplementedException();

    public bool UpdateSettings(string? ipAddress, string? userName, string? password, ClockSettings settings)
    {
        if (!IsAdministrator(password)) return false;
        UpdateUser(ipAddress, userName);
        var updated = UpdateSettings(settings);
        if (updated)
        {
            if (OnUpdate is not null) OnUpdate(this, Name);
            _logger.LogInformation("Clock '{ClockName}' settings was updated by {UserName}.", Name, userName);
        }
        return updated;
    }

    private bool UpdateSettings(ClockSettings settings)
    {
        if (settings is null) return false;
        if (Name.IsNotSameAs(settings.Name)) return false;
        if (settings.AdministratorPassword.HasValue()) AdministratorPassword = settings.AdministratorPassword;
        if (settings.UserPassword.HasValue()) UserPassword = settings.UserPassword;
        Mode = settings.Mode;
        StartWeekday = settings.StartWeekday;
        StartDayAndTime = new TimeSpan((int)settings.StartWeekday, settings.StartTime.Hour, settings.StartTime.Minute);
        Duration = settings.Duration;
        Speed = settings.Speed;
        PauseTime = settings.PauseTime;
        PauseReason = settings.PauseReason;
        ResumeAfterPauseTime = settings.ResumeAfterPauseTime;
        ShowRealtimeDuringPause = settings.ShowRealtimeDuringPause;
        Elapsed = settings.OverriddenElapsedTime.HasValue ? settings.OverriddenElapsedTime.Value - settings.StartTime : Elapsed;
        Message = settings.Message;
        if (settings.ShouldReset) Reset();
        if (settings.IsRunning) TryStartTick(StoppingUser, AdministratorPassword); else { StopTick(); }

        return true;
    }
    public bool UpdateUser(string? ipAddress, string? userName, string clientVersion = "")
    {
        const string Unknown = nameof(Unknown); 
        if (string.IsNullOrWhiteSpace(userName)) userName = Unknown;
        lock (_clients)
        {
            if (ipAddress is null) return false;
            var unknown = _clients.FirstOrDefault(e => ipAddress.Equals(e.IPAddress) && Unknown.Equals(e.UserName, StringComparison.OrdinalIgnoreCase));
            if (unknown is not null)
            {
                unknown.Update(userName, clientVersion);
                return true;
            }

            var existing = _clients.Where(c => ipAddress.Equals(c.IPAddress) && userName.Equals(c.UserName, StringComparison.OrdinalIgnoreCase)).ToArray();
            if (existing.Length == 0)
            {
                _clients.Add(new ClockUser() { IPAddress = ipAddress, UserName = userName, ClientVersion = clientVersion });
                _logger.LogInformation("Clock '{name}' has new user '{userName}' from IP-address '{ipadress}'", Name, userName, ipAddress);
                return true;
            }
            if (existing.Length >= 1) existing[0].Update(userName, clientVersion);
            return true;
        }
    }

    public bool IsUser(string? password) =>
        string.IsNullOrWhiteSpace(UserPassword) && string.IsNullOrWhiteSpace(password) ||
        IsAdministrator(password) ||
        !string.IsNullOrWhiteSpace(password) && password.Equals(UserPassword, StringComparison.OrdinalIgnoreCase);
    public bool IsAdministrator(string? password) => 
        !string.IsNullOrWhiteSpace(password) && 
        password.Equals(AdministratorPassword, StringComparison.OrdinalIgnoreCase);

    public IClock StartServer(ClockSettings settings)
    {
        UpdateSettings(settings);
        return this;
    }

    public bool TryStartTick(string? userName, string? password)
    {
        if (IsRunning) return true;
        if (IsStoppingUser(userName, password) || IsAdministrator(password))
        {
            if (IsAdministrator(password)) ResetPause();
            ResetStopping();
            _clockTimer.Start();
            IsRunning = true;
            return true;
        }
        return false;
    }
    public bool TryStopTick(string? userName, string? password, StopReason reason) => throw new NotImplementedException();

    private void Reset()
    {
        Elapsed = TimeSpan.Zero;
        IsRunning = false;
        ResetPause();
        ResetStopping();
        _logger.LogInformation("Clock {name} was resetted", Name);

    }

    private void ResetPause()
    {
        if (IsPaused)
        {
            IsPaused = false;
            ShowRealtimeDuringPause = false;
            PauseTime = null;
            ResumeAfterPauseTime = null;
            PauseReason = PauseReason.None;
            Mode = ClockMode.Fast;
        }
    }

    private void ResetStopping()
    {
        StoppingUser = null;
        StopReason = StopReason.None;
    }

    public void StopTick()
    {
        if (!IsRunning) return;
        _clockTimer.Stop();
        IsRunning = false;
    }

    private void Tick(object? me, ElapsedEventArgs args)
    {
        IncreaseTime();
        if (RealTime >= PauseTime)
        {
            IsPaused = true;
            Mode = ShowRealtimeDuringPause ? ClockMode.Real : ClockMode.Fast;
            StopTick();
        }
        if (IsCompleted)
            StopTick();
    }

    private void IncreaseTime()
    {
        var previousElapsed = Elapsed;
        Elapsed = Elapsed.Add(TimeSpan.FromSeconds(Speed));
        if (Elapsed.Minutes != previousElapsed.Minutes)
            if (OnUpdate is not null) OnUpdate(this, Name);
    }

    private bool IsStoppingUser(string? userName, string? password) =>
        IsUser(password) && userName.IsSameAs(StoppingUser);
  

    internal bool IsRunning { get; private set; }
    internal bool IsPaused { get; private set; }
    internal bool IsCompleted => Elapsed >= Duration;
    internal bool IsRealtime => Mode.IsReal();

    internal TimeOnly RealTime { get; private set; }

    internal string AdministratorPassword { get; set; }
    internal string UserPassword { get; private set; }
    internal ClockMode Mode { get; private set; }   
    internal Weekday StartWeekday { get; private set; }
    internal TimeSpan StartDayAndTime { get; private set; }
    internal TimeSpan Duration { get; private set; }
    internal TimeSpan Elapsed { get; private set; }
    internal double Speed { get; private set; } 
    internal TimeOnly? PauseTime { get; private set; }
    internal PauseReason PauseReason { get; private set; }
    internal TimeOnly? ResumeAfterPauseTime { get; private set; }
    internal bool ShowRealtimeDuringPause { get; private set; }
    internal StopReason StopReason { get; private set; }
    internal string? StoppingUser { get; private set; }
    internal string? Message { get; private set; }

    #region Dispose
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    private bool disposedValue;
    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
