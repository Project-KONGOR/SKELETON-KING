namespace ZORGATH;

/// <summary>
///     Response returned by the server when login fails.
/// </summary>
public class AuthFailedResponse
{
    public AuthFailedResponse(AuthFailureReason reason)
    {
        AuthOutcome = reason switch
        {
            AuthFailureReason.AccountNotFound => "Account Not Found",
            AuthFailureReason.InvalidSession => "Invalid Session (Try Logging In Again)",
            AuthFailureReason.IncorrectPassword => "Incorrect Password",
            _ => $@"Unsupported Authentication Failure Reason ""{nameof(reason)}"""
        };
    }

    /// <summary>
    ///     A string of error output in the event of an auth failure. E.g. "Invalid Nickname or Password.".
    /// </summary>
    [PhpProperty("auth")]
    public readonly string AuthOutcome;

    /// <summary>
    ///     Unknown property which seems to be set to true on a successful response or false if an error occurs.
    ///     Since this is an error response, set to "false".
    /// </summary>
    [PhpProperty(0)]
    public readonly bool Zero = false;
}

public enum AuthFailureReason
{
    AccountNotFound,
    InvalidSession,
    IncorrectPassword,
}
