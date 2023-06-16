using System.ComponentModel.DataAnnotations.Schema;

namespace PUZZLEBOX;

/// <summary>
///     IncidentType values map 1-to-1 with the names and indices of the report-a-player options in the in-game UI (apart from the Invalid option).
/// </summary>
public enum IncidentType
{
    Invalid = 0,
    ChatAbuse = 1,
    FeedingOrStatManipulation = 2,
    ImpersonatingStaff = 3,
    AbilityOrItemAbuse = 4,
    BadNicknameOrClanName = 5,
    RefusalToParticipateOrAfk = 6
}

/// <summary>
///     IncidentStatus tracks the state machine flow for a report.
///     Reports are expected to default to the "New" status. And then flow through the "UnderReview" state until an eventual resolution (e.g. "Rejected" or "Done").
/// </summary>
public enum IncidentStatus
{
    // The report has been submitted, but has not been acted upon yet.
    New,

    // The report is under review by a moderator.
    UnderReview,

    // The report was rejected, e.g. because it was spam.
    Rejected,

    // The report was processed by a moderator, and is now done.
    Done
}

public class IncidentReport
{
    /// <summary>
    ///     The primary key to identify the incident report by.
    /// </summary>
    [Required]
    public int IncidentReportId { get; set; }

    /// <summary>
    ///     A navigation property to the account that submitted the report.
    /// </summary>
    [ForeignKey("ReporterId")]
    public Account? Reporter { get; set; }

    /// <summary>
    ///     A foreign key storing the ID of the account that submitted the report.
    /// </summary>
    [Required]
    public int? ReporterId { get; set; }

    /// <summary>
    ///     A navigation property to the account that is being reported.
    /// </summary>
    [ForeignKey("AccusedId")]
    public Account? Accused { get; set; }

    /// <summary>
    ///     A foreign key storing the ID of the account that is being reported.
    /// </summary>
    [Required]
    public int? AccusedId { get; set; }

    /// <summary>
    ///     A navigation property to the account that is reviewing the report.
    /// </summary>
    [ForeignKey("ReviewerId")]
    public Account? Reviewer { get; set; }

    /// <summary>
    ///     A foreign key storing the ID of the account that is reviewing the report.
    /// </summary>
    public int? ReviewerId { get; set; }

    /// <summary>
    ///     A foreign key to the match ID associated with this report.
    /// </summary>
    [Required]
    public int MatchId { get; set; }

    /// <summary>
    ///     The type of the reported incident.
    /// </summary>
    [Required]
    public IncidentType IncidentType { get; set; }

    /// <summary>
    ///     The body text of the report.
    /// </summary>
    [Required]
    public string? Description { get; set; }

    /// <summary>
    ///     The match time, in minutes, at which the incident occurred.
    ///     This is the value controlled by the time slider in the report-a-player form in the client.
    ///     The in-game UI displays the time using fractional time values. But the request sent to the server only contains the value truncated to the precision of minutes.
    /// </summary>
    [Required]
    public int OffenseOccurrenceTime { get; set; }

    /// <summary>
    ///     The current status of the report. Defaults to "IncidentStatus.New".
    /// </summary>
    [Required]
    public IncidentStatus Status { get; set; } = IncidentStatus.New;

    /// <summary>
    ///     The outcome of the report. This field should be set with any notes on the outcome of the report, once the report is acted upon by a moderator.
    /// </summary>
    public string? ResolutionDetails { get; set; }
}
