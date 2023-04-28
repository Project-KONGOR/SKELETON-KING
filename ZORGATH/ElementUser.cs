namespace ZORGATH;

/// <summary>
///     An Entity Framework object representing an owner of one or more Accounts. Includes information shared by all
///     accounts, such as password-related information and any of the content they have unlocked.
/// </summary>
[Index(nameof(UserName), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class ElementUser : IdentityUser
{
    public ElementUser(string salt, string passwordSalt, string hashedPassword)
    {
        Salt = salt;
        PasswordSalt = passwordSalt;
        HashedPassword = hashedPassword;
    }

    /// <summary>
    ///     The SRP "salt" value for the client to use during authentication.
    /// </summary>
    [Required]
    public string Salt { get; set; }

    /// <summary>
    ///     The HoN specific "salt" value used in the password hashing algorithm by the client.
    ///     Referred to by the HoN client as `salt2`.
    /// </summary>
    [Required]
    public string PasswordSalt { get; set; }

    [Required]
    public string HashedPassword { get; set; }
}
