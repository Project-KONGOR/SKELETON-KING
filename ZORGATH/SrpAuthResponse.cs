namespace ZORGATH;

/// <summary>
///     The auth response for HoN SRP authentication.
/// </summary>
[Serializable]
public class SrpAuthResponse : AuthResponse
{
    public SrpAuthResponse(AccountDetails accountDetails, string cookie, string clientIpAddress, long hostTime, string chatServerUrl, string icbUrl, string proof, SecInfo secInfo) : base(accountDetails, cookie, clientIpAddress, hostTime, chatServerUrl, icbUrl, secInfo)
    {
        Proof = proof;
    }

    /// <summary>
    ///     The "M1" client "proof" value for SRP authentication.
    ///     The server should verify this and reply back with "M2".
    /// </summary>
    [PhpProperty("proof")]
    public readonly string Proof;
}