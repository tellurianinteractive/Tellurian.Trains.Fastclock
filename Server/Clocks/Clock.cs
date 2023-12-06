using Fastclock.Contracts;
using Fastclock.Contracts.Extensions;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Timers;
using static Fastclock.Contracts.Extensions.TimeZoneInfoExtensions;
using Timer = System.Timers.Timer;

[assembly: InternalsVisibleTo("Fastclock.Server.Tests")]

namespace Fastclock.Server.Clocks;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class Clock : IDisposable, IClock
{
    public const string DemoClockName = "Demo";

    private readonly ClockConfiguration _configuration;
    private readonly ILogger<Clock> _logger;
    private readonly Timer _clockTimer;
    private readonly List<ClockUser> _clients = [];
    private TimeZoneInfo _timeZone;

    public Clock(IOptions<ClockConfiguration> options, ILogger<Clock> logger)
    {
        _configuration = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;
        _timeZone = CreateTimeZoneInfo(_configuration.TimeZoneId);
        Name = _configuration.Name;
        AdministratorPassword = _configuration.Password;
        StartWeekday = _configuration.StartWeekday;
        StartTime = _configuration.StartTime;
        SessionDuration = _configuration.Duration;
        Speed = _configuration.Speed;
        UserPassword = string.Empty;
        _clockTimer = new Timer(1000);
        _clockTimer.Elapsed += Tick;
    }


    public event EventHandler<string>? OnUpdate;
    public string Name { get; init; }
    public ClockSettings Settings { get; init; } = new ClockSettings();
    public ClockStatus Status => new()
    {
        ClockMode = Mode,
        Duration = SessionDuration,
        ResumeTimeAfterPause = ResumeAfterPauseTime,
        FastEndTime = FastEndTime,
        IsCompleted = IsCompleted,
        IsElapsed = Elapsed > StartTime.ToTimeSpan(),
        IsPaused = IsPaused,
        IsRunning = IsRunning,
        Message = Message,
        Name = Name,
        PauseReason = PauseReason,
        PauseTime = PauseTime,
        RealEndTime = RealEndTime,
        Speed = Speed,
        StoppedByUser = StoppingUser ?? "",
        StoppingReason = StoppingReason,
        Weekday = Weekday,
        Time = Time,
    };

    public IEnumerable<ClockUser> ClockUsers => _clients;

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
        if (!IsAdministrator(settings.AdministratorPassword)) return false;
        if (Name.IsNotSameAs(DemoClockName))
        {
            if (settings.AdministratorPassword.HasValue()) AdministratorPassword = settings.AdministratorPassword;
            if (settings.UserPassword.HasValue()) UserPassword = settings.UserPassword;
        }
        Mode = settings.Mode;
        StartWeekday = settings.StartWeekday;
        StartTime = settings.StartTime;
        StartDayAndTime = new TimeSpan((int)settings.StartWeekday, settings.StartTime.Hour, settings.StartTime.Minute);
        SessionDuration = settings.Duration;
        Speed = settings.Speed;
        PauseTime = settings.PauseTime;
        PauseReason = settings.PauseReason;
        ResumeAfterPauseTime = settings.ResumeAfterPauseTime;
        ShowRealtimeDuringPause = settings.ShowRealtimeDuringPause;
        Elapsed = settings.OverriddenElapsedTime > settings.StartTime ? settings.OverriddenElapsedTime.Value - settings.StartTime : Elapsed;
        Message = settings.Message;
        _timeZone = CreateTimeZoneInfo(settings.TimeZoneId);
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
                var user = new ClockUser()
                {
                    IPAddress = ipAddress,
                    UserName = userName,
                    ClientVersion = clientVersion,
                    LastClockAccessTimestamp = DateTimeOffset.Now
                };
                _clients.Add(user);
                _logger.LogInformation("Clock '{name}' has new user '{userName}' from IP-address '{ipadress}'", Name, userName, ipAddress);
                return true;
            }
            if (existing.Length >= 1) existing[0].Update(userName, clientVersion);
            return true;
        }
    }

    public bool IsUser(string? password, bool requirePassword = false) =>
        IsAdministrator(password) ||
        requirePassword ? UserPassword.HasValue() && password.IsSameAs(UserPassword) :
        password.IsSameAs(UserPassword);

    public bool IsAdministrator(string? password) => password.HasValue() && password.IsSameAs(AdministratorPassword);

    public bool TryStartTick(string? userName, string? password)
    {
        if (IsRunning) return true;
        if (IsPaused && IsAdministrator(password))
        {
            ResetPause();
            ResetStopping();
            _clockTimer.Start();
            IsRunning = true;
        }
        else if (IsStoppingUser(userName, password) || IsAdministrator(password))
        {
            ResetStopping();
            _clockTimer.Start();
            IsRunning = true;
        }
        return IsRunning;
    }
    public bool TryStopTick(string userName, string? password, StopReason reason)
    {
        if (IsRunning && IsUser(password))
        {
            StopTick(userName, reason);
            return true;
        }
        return false;
    }
    public void StopTick(string userName, StopReason reason)
    {
        if (!IsRunning) return;
        StoppingReason = reason;
        StoppingUser = userName;
        StopTick();
        _logger.LogInformation("Clock {ClockName} was stopped by {UserName}", Name, userName);

    }
    public void StopTick()
    {
        if (!IsRunning) return;
        _clockTimer.Stop();
        IsRunning = false;
    }
    private void Reset()
    {
        Elapsed = TimeSpan.Zero;
        IsRunning = false;
        ResetPause();
        ResetStopping();
        _logger.LogInformation("Clock {ClockName} was resetted", Name);
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
        StoppingReason = StopReason.None;
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
    internal bool IsCompleted => Elapsed >= SessionDuration;
    internal bool IsRealtime => Mode.IsReal();

    internal string AdministratorPassword { get; set; }
    internal string UserPassword { get; private set; }
    internal ClockMode Mode { get; private set; }
    internal Weekday StartWeekday { get; private set; }
    internal Weekday Weekday => StartWeekday == Weekday.None ? Weekday.None : (Weekday)((int)StartWeekday + StartTime.Hour / 24 + Elapsed.Hours / 24);
    internal TimeOnly StartTime { get; private set; }
    internal TimeSpan StartDayAndTime { get; private set; }
    internal TimeSpan SessionDuration { get; private set; }
    internal TimeSpan Elapsed { get; private set; }
    internal TimeOnly Time => StartTime.Add(Elapsed, out _);
    internal double Speed { get; private set; }
    internal TimeOnly? PauseTime { get; private set; }
    internal PauseReason PauseReason { get; private set; }
    internal TimeOnly? ResumeAfterPauseTime { get; private set; }
    internal bool ShowRealtimeDuringPause { get; private set; }
    internal StopReason StoppingReason { get; private set; }
    internal string? StoppingUser { get; private set; }
    internal string? Message { get; private set; }

    internal TimeOnly FastEndTime => StartTime.Add(SessionDuration);
    internal TimeSpan PauseDuration =>
        PauseTime.HasValue && ResumeAfterPauseTime.HasValue ? ResumeAfterPauseTime.Value - PauseTime.Value : TimeSpan.Zero;

    internal TimeOnly RealEndTime => RealTime.AddMinutes((SessionDuration.TotalMinutes / Speed) + PauseDuration.TotalMinutes);
    internal TimeOnly RealTime
    {
        get
        {
            var now = RealDayAndTime;
            return new TimeOnly(now.Hours, now.Minutes);
        }
    }
    internal TimeSpan RealDayAndTime
    {
        get
        {
            var utcOffset = _timeZone.GetUtcOffset(DateTimeOffset.Now);
            var now = DateTime.UtcNow + utcOffset;
            var day = (int)now.DayOfWeek;
            return new TimeSpan(day == 0 ? 7 : day, now.Hour, now.Minute, now.Second);
        }
    }

    #region Dispose
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    private bool _disposedValue;
    private void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _clockTimer?.Dispose();
            }
            _disposedValue = true;
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
