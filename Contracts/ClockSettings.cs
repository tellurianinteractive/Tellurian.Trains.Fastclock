using System.ComponentModel.DataAnnotations;
using Fastclock.Contracts.Resources;

namespace Fastclock.Contracts;

public record ClockSettings
{

    /// <summary>
    /// Name of clock. If a non-extisting clock name is given, a new clock instance is created.
    /// </summary>
    [Display(Name = "ClockName", ResourceType = typeof(Strings))]
    [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Strings))]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Type of time to display.
    /// </summary>
    [Display(Name = "ClockMode", ResourceType = typeof(Strings))]
    [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Strings))]
    public ClockMode Mode { get; set; }
    /// <summary>
    /// True if clock should resume game from start time.
    /// </summary>
    [Display(Name = nameof(ShouldReset), ResourceType = typeof(Strings))]
    public bool ShouldReset { get; set; }
    /// <summary>
    /// True if current game time is later than game start time.
    /// </summary>
    [Display(Name = nameof(IsElapsed), ResourceType = typeof(Strings))]
    public bool IsElapsed { get; set; }
    /// <summary>
    /// True if clock is running (or should be running).
    /// </summary>
    [Display(Name = nameof(IsRunning), ResourceType = typeof(Strings))]
    public bool IsRunning { get; set; }
    /// <summary>
    /// The weekday that the game should start at. Weekdays are defined in <see cref="Weekday"/>.
    /// </summary>
    [Display(Name = nameof(StartWeekday), ResourceType = typeof(Strings))]
    public Weekday StartWeekday { get; set; }
    /// <summary>
    /// The game start time.
    /// </summary>
    [Display(Name = nameof(StartTime), ResourceType = typeof(Strings))]
    [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Strings))]
    [RegularExpression("(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessageResourceName = "InvalidTime", ErrorMessageResourceType = typeof(Strings))]
    public TimeOnly StartTime { get; set; }
    /// <summary>
    /// The game time speed. Decimal value.
    /// </summary>
    [Display(Name = nameof(Speed), ResourceType = typeof(Strings))]
    [Range(1.0, 7.0, ErrorMessageResourceName = "InvalidRange", ErrorMessageResourceType = typeof(Strings))]
    public double Speed { get; set; }
    /// <summary>
    /// Duration of the game in hours (with decimals).
    /// </summary>
    [Display(Name = nameof(Duration), ResourceType = typeof(Strings))]
    [Range(1.0, 168.0, ErrorMessageResourceName = "InvalidRange", ErrorMessageResourceType = typeof(Strings))]
    public TimeSpan Duration { get; set; }
    /// <summary>
    /// Real time when pause starts.
    /// </summary>
    [Display(Name = nameof(PauseTime), ResourceType = typeof(Strings))]
    [RegularExpression("(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessageResourceName = "InvalidTime", ErrorMessageResourceType = typeof(Strings))]
    public TimeOnly? PauseTime { get; set; }
    /// <summary>
    /// Real time when pause starts.
    /// </summary>
    [Display(Name = nameof(PauseReason), ResourceType = typeof(Strings))]
    public PauseReason PauseReason { get; set; }
    /// <summary>
    /// Expected real time when game will resume or empty.
    /// </summary>
    [Display(Name = nameof(ResumeAfterPauseTime), ResourceType = typeof(Strings))]
    [RegularExpression("(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessageResourceName = "InvalidTime", ErrorMessageResourceType = typeof(Strings))]
    public TimeOnly? ResumeAfterPauseTime { get; set; }
    /// <summary>
    /// True if real time should be shown during pause.
    /// </summary>
    [Display(Name = nameof(ShowRealtimeDuringPause), ResourceType = typeof(Strings))]
    public bool ShowRealtimeDuringPause { get; set; }
    /// <summary>
    /// Option to change the current elapsed game time, for example if clock is stopped to late and game time have to be set back in time.
    /// </summary>
    [Display(Name = nameof(OverriddenElapsedTime), ResourceType = typeof(Strings))]
    [RegularExpression("(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessageResourceName = "InvalidTime", ErrorMessageResourceType = typeof(Strings))]
    public TimeOnly? OverriddenElapsedTime { get; set; }
    /// <summary>
    /// Eventually manual entered message by the administrator to display.
    /// </summary>
    [Display(Name = nameof(Message), ResourceType = typeof(Strings))]
    [StringLength(100, ErrorMessageResourceName = "InvalidString", ErrorMessageResourceType = typeof(Strings))]
    public string? Message { get; set; } 
   /// <summary>
    /// Clock administrator password.
    /// </summary>
    [Display(Name = nameof(AdministratorPassword), ResourceType = typeof(Strings))]
    [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Strings))]
    [StringLength(10, ErrorMessageResourceName = "InvalidString", ErrorMessageResourceType = typeof(Strings))]
    public string? AdministratorPassword { get; set; }
    /// <summary>
    /// User administrator password.
    /// </summary>
    [Display(Name = nameof(UserPassword), ResourceType = typeof(Strings))]
    [StringLength(10, ErrorMessageResourceName = "InvalidString", ErrorMessageResourceType = typeof(Strings))]
    public string UserPassword { get; set; } = string.Empty;
}
