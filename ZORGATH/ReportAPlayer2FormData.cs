namespace ZORGATH;

/// <summary>
///     This form data is used by the modern RAP flow, which is the current default
///     for the HoN client.
/// </summary>
public class ReportAPlayer2FormData
{
    /// <summary>
    ///     The ID of the player filing the report
    /// </summary>
    [FromForm(Name = "account_id")]
    public int AccountId { get; set; }

    /// <summary>
    ///     The client's cookie, used to authorize the session.
    /// </summary>
    public string? Cookie { get; set; }

    /// <summary>
    ///     The name of the user being reported
    /// </summary>
    [FromForm(Name = "offenderName")]
    public string? OffenderName { get; set; }

    /// <summary>
    ///     The ID of the match in which the alleged incident occurred.
    /// </summary>
    [FromForm(Name = "match_id")]
    public int MatchId { get; set; }

    /// <summary>
    ///     The type of incident being reported.
    /// </summary>
    public IncidentType Type { get; set; }

    /// <summary>
    ///     The body of text describing what report accuses the `OffenderName` of doing wrong.
    /// </summary>
    public string? Body { get; set; }

    /// <summary>
    ///     The match time, in minutes, at which the incident occurred.
    ///     
    ///     This is the value controlled by the time slider in the report-a-player form in the client.
    ///     
    ///     NOTE: The in-game UI displays the time using fractional time values. But the request sent to the server only
    ///     contains the value truncated to the precision of minutes.
    /// </summary>
    public int Time { get; set; }
}
