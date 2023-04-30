public class AccountLookupErrorResponse
{
    /// <summary>
    ///     The error message for the response.
    /// </summary>
    [PhpProperty("error")]
    public readonly string Error = "FALSE";

    /// <summary>
    ///     Unknown property which seems to be set to "0" (false) on an error response.
    /// </summary>
    [PhpProperty(0)]
    public readonly bool Zero = false;
}