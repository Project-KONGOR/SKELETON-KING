namespace ZORGATH.GameAPI.Features.Auth.Helpers;

public record AccountRegistrationData(string Salt, string PasswordSalt, string HashedPassword);

// Ported from KONGOR OG codebase /tarrexd
public static class SrpRegistrationHelpers
{
    // The official HoN authentication server uses these in SRP. I took them from existing projects,
    // so not sure how they were reverse engineered.
    // Courtesy of https://github.com/theli-ua/pyHoNBot/blob/master/hon/masterserver.py#L37
    private const string magic = "[!~esTo0}";
    private const string magic2 = "taquzaph_?98phab&junaj=z=kuChusu";

    /**
     * This method simulates the password hashing algorithm used by the HoN clients during registration.
     * This logic was ported pretty directly from:
     * https://github.com/theli-ua/pyHoNBot/blob/master/hon/masterserver.py#L37
     */
    public static string HashAccountPassword(string password, string passwordSalt)
    {
        string md5Pass = Convert.ToHexString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password))).ToLower();
        return HashAccountPasswordMD5(md5Pass, passwordSalt);
    }

    public static string HashAccountPasswordMD5(string md5Pass, string passwordSalt)
    {
        string saltedMagic = md5Pass + passwordSalt + magic;
        string md5Magic = Convert.ToHexString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(saltedMagic))).ToLower();
        string magicTwo = md5Magic + magic2;
        return Convert.ToHexString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(magicTwo))).ToLower();
    }

    /// <summary>
    ///     Generates a password salt of a specified hex length.
    ///     The default value is 22, for the purpose of consistency with the original HoN salt hex length.
    /// </summary>
    private static string GeneratePasswordSalt(int saltHexLength = 22)
    {
        // SrpInteger expects a byte length, so divide the saltLength by 2
        // since there are 2 hex digits per byte.
        int saltLengthInBytes = saltHexLength / 2;

        return SrpInteger.RandomInteger(saltLengthInBytes).ToHex();
    }

    public static AccountRegistrationData GenerateAccountRegistrationData(string password)
    {
        string salt = SrpInteger.RandomInteger(SrpAuthSessionData.N.Length/2).ToHex();
        string passwordSalt = GeneratePasswordSalt();
        string hashedPassword = HashAccountPassword(password, passwordSalt);
        return new AccountRegistrationData(Salt: salt, PasswordSalt: passwordSalt, HashedPassword: hashedPassword);
    }
}
