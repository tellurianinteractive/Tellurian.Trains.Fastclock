namespace Fastclock.Contracts;

/// <summary>
/// Data for presentation of clock time and status.
/// All data is in English-only. It is the clients responibility to make translations.
/// </summary>
public record ClockStatus
{
    /// <summary>
    /// Name of selected clock.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Shoud be set to true when API-data not is available.
    /// </summary>
    public bool IsUnavailable { get; set; }
    /// <summary>
    /// Eventually manual entered message by the administrator to display.
    /// </summary>
    public string? Message { get; set; }

    public Session Session { get; set; } = new Session();
    public Realtime Realtime { get; set; } = new Realtime();
    public Stopping? Stopping { get; set; } 
    public Pause? Pause { get; set; } 
   public string ServerVersionNumber { get; set; } = string.Empty;
    /// <summary>
    /// IP-address of host running clock.
    /// </summary>
}


/// <summary>
/// All properties for real time in the defined time zone.
/// </summary>
public record Realtime
{
    /// <summary>
    /// Name of game weekday or empty.
    /// </summary>
    public Weekday Weekday { get; set; }
    /// <summary>
    /// Current real time.
    /// </summary>
    public TimeOnly Time { get; set; }
}

/// <summary>
/// All properties for session/fast clock.
/// </summary>
public record Session
{
    /// <summary>
    /// Fast clock weekday
    /// </summary>
    public Weekday Weekday { get; set; }
    /// <summary>
    /// Current fast time.
    /// </summary>
    public TimeOnly Time { get; set; }
    /// <summary>
    /// Clock speed where values over 1 is faster than real time.
    /// </summary>
    public Speed Speed { get; set; }
    /// <summary>
    /// Game duration in hours (with decimals).
    /// </summary>
    public TimeSpan Duration { get; set; }
    /// <summary>
    /// Fast time when clock will stop for session break.
    /// </summary>
    public TimeOnly? BreakTime { get; set; }
    /// <summary>
    /// Real break time or real end time of session, including time of pause if pause times have been set.
    /// </summary>
    public TimeOnly RealEndTime { get; set; }
    /// <summary>
    /// If set: break time ; otherwise start time plus dusation.
    /// </summary>
    public TimeOnly FastEndTime { get; set; }
    /// <summary>
    /// True if fast time has reached the end time.
    /// </summary>
    public bool IsCompleted { get; set; }
    /// <summary>
    /// True if fast clock has been initially started.
    /// </summary>
    public bool IsElapsed { get; set; }
    /// <summary>
    /// True if fast clock is running.
    /// </summary>
    public bool IsRunning { get; set; }
}

/// <summary>
/// Properties valid when the fast clock is stopped for some reason during a session.
/// </summary>
public record Stopping
{
    /// <summary>
    /// Name of user or station that have stopped the game time.
    /// </summary>
    public string? UserName { get; set; }
    /// <summary>
    /// Reason for stopping game time. These are from the set of values in <see cref="StoppingReason"/>.
    /// </summary>
    public StopReason Reason { get; set; }
}

/// <summary>
/// Properties for pauses in real time.
/// </summary>
public record Pause
{
    /// <summary>
    /// True if game time is paused. It is recommended to present when and why the game is paused and will resume.
    /// </summary>
    public bool IsPaused { get; set; }
    /// <summary>
    /// Reason for pause. These are from the set of values in <see cref="Reason"/>.
    /// </summary>
    public PauseReason Reason { get; set; }
    /// <summary>
    /// Real time when clock will stop for pause.
    /// </summary>
    public TimeOnly? Time { get; set; }
    /// <summary>
    /// Expected real time when game will resume or empty.
    /// </summary>
    public TimeOnly? SessionResumeTime { get; set; }

}


