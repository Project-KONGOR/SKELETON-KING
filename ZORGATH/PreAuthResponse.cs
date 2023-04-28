namespace ZORGATH;

[Serializable]
public class PreAuthResponse
{
    public PreAuthResponse(string salt, string passwordSalt, string serverPublicEphemeral)
    {
        Salt = salt;
        Salt2 = passwordSalt;
        B = serverPublicEphemeral;
    }

    /// <summary>
    ///     The SRP "salt" value for the client to use during authentication.
    ///     Not sent in the event of a invalid username.
    /// </summary>
    [PhpProperty("salt")]
    public readonly string Salt;

    /// <summary>
    ///     The HoN specific "salt" value used in the password hashing algorithm by the client.
    ///     Not sent in the event of a invalid username.
    /// </summary>
    [PhpProperty("salt2")]
    public readonly string Salt2;

    /// <summary>
    ///     The ephemeral "B" value, created by the server and sent to the client for use during SRP authentication.
    /// </summary>
    [PhpProperty("B")]
    public readonly string B;

    /// <summary>
    ///     Unknown property which seems to often be set to "5", for some reason.
    /// </summary>
    [PhpProperty("vested_threshold")]
    public readonly int VestedThreshold = 5;

    /// <summary>
    ///     Unknown property which seems to be set to true on a successful response or false if an error occurs.
    ///     If an error occurred, use "AuthFailedResponse" instead.
    /// </summary>
    [PhpProperty(0)]
    public readonly bool Zero = true;
}
