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
    /// Name of game weekday or empty.
    /// </summary>
    public Weekday Weekday { get; set; }
    /// <summary>
    /// The type of time that is provided
    /// </summary>
    public ClockMode ClockMode { get; set; }
    /// <summary>
    /// Current time to display. The type of time is dependent of <see cref="IsRealtime"/>.
    /// </summary>
    public TimeOnly Time { get; set; }
    /// <summary>
    /// Game duration in hours (with decimals).
    /// </summary>
    public TimeSpan Duration { get; set; }
    /// <summary>
    /// Clock speed where values over 1 is faster than real time.
    /// </summary>
    public Speed Speed { get; set; }
    /// <summary>
    /// True if clock is running. This is always true if clock is running in realtime.
    /// </summary>
    public bool IsRunning { get; set; }
    /// <summary>
    /// True if game time has reached the end time. Always false when running in realtime.
    /// </summary>
    public bool IsCompleted { get; set; }
    /// <summary>
    /// True if clock has been initially started.
    /// </summary>
    public bool IsElapsed { get; set; }
    /// <summary>
    /// Shoud be set to true when API-data not is available.
    /// </summary>
    public bool IsUnavailable { get; set; }
    /// <summary>
    /// Forecasted real end time of game- Includes time of pause if pause times have been set.
    /// </summary>
    public TimeOnly RealEndTime { get; set; }
    /// <summary>
    /// Game end time.
    /// </summary>
    public TimeOnly FastEndTime { get; set; }
    /// <summary>
    /// True if game time is paused. It is recommended to present when and why the game is paused and will resume.
    /// </summary>
    public bool IsPaused { get; set; }
    /// <summary>
    /// Reason for pause. These are from the set of values in <see cref="PauseReason"/>.
    /// </summary>
    public PauseReason PauseReason { get; set; }
    /// <summary>
    /// Real time when pause starts.
    /// </summary>
    public TimeOnly? PauseTime { get; set; }
    /// <summary>
    /// Expected real time when game will resume or empty.
    /// </summary>
    public TimeOnly? ResumeTimeAfterPause { get; set; }
    /// <summary>
    /// Name of user or station that have stopped the game time.
    /// </summary>
    public string? StoppedByUser { get; set; }
    /// <summary>
    /// Reason for stopping game time. These are from the set of values in <see cref="StoppingReason"/>.
    /// </summary>
    public StopReason StoppingReason { get; set; }
    /// <summary>
    /// Eventually manual entered message by the administrator to display.
    /// </summary>
    public string? Message { get; set; }
    /// Current server application version. This can be used to verify client application compatibility.
    /// </summary>
    public string ServerVersionNumber { get; set; } = string.Empty;
    /// <summary>
    /// IP-address of host running clock.
    /// </summary>
    public string HostAddress { get; set; } = string.Empty;
}


