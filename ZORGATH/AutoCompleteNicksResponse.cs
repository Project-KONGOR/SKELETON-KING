public class AutoCompleteNicksResponse
{
    public AutoCompleteNicksResponse(List<string> matchedAccountNames) => MatchedAccountNames = matchedAccountNames;

    /// <summary>
    ///     A sorted list of the matching nicknames that match the auto-complete.
    /// </summary>
    [PhpProperty("nicks")]
    public readonly List<string> MatchedAccountNames;

    /// <summary>
    ///     Unknown property which seems to often be set to "5", for some reason.
    /// </summary>
    [PhpProperty("vested_threshold")]
    public readonly int VestedThreshold = 5;

    /// <summary>
    ///     Unknown property which seems to be set to "1" (true) on a successful response.
    /// </summary>
    [PhpProperty(0)]
    public readonly bool Zero = true;
}
