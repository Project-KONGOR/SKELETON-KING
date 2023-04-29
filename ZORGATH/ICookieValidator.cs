namespace ZORGATH;

/// <summary>
///     A service interface to consolidate cookie authentication.
/// </summary>
public interface ICookieValidator
{
    /// <summary>
    ///     Validates the cookie and returns the corresponding Session. In the event of an
    ///     invalid cookie, returns null.
    /// </summary>
    public Task<Account?> ValidateSessionCookie(string sessionCookie);

    /// <summary>
    ///     Validates the cookie and accountID, and returns the corresponding Session. In
    ///     the event of an invalid cookie, returns null.
    /// </summary>
    public Task<Account?> ValidateSessionCookieForAccountId(string sessionCookie, string accountID);
}