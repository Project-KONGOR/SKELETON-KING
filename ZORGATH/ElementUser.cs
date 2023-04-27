namespace ZORGATH;

/// <summary>
///     An Entity Framework object representing an owner of one or more Accounts. Includes information shared by all
///     accounts, such as password-related information and any of the content they have unlocked.
/// </summary>
[Index(nameof(UserName), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class ElementUser : IdentityUser
{
    public ElementUser(string salt, string passwordSalt, string hashedPassword, string email, int goldCoins, int silverCoins, int plinkoTickets, ICollection<string> unlockedUpgradeCodes)
        : this(salt, passwordSalt, hashedPassword, email, unlockedUpgradeCodes)
    {
        GoldCoins = goldCoins;
        SilverCoins = silverCoins;
        PlinkoTickets = plinkoTickets;
    }

    // Convenient method for testing.
    public ElementUser(string salt = "salt", string passwordSalt = "passwordSalt", string hashedPassword = "hashedPassword", string email = "email", ICollection<string>? unlockedUpgradeCodes = null)
    {
        Salt = salt;
        PasswordSalt = passwordSalt;
        HashedPassword = hashedPassword;
        UnlockedUpgradeCodes = unlockedUpgradeCodes ?? Array.Empty<string>();

        // Base Properties
        Email = email;
        EmailConfirmed = true;
    }

    /// <summary>
    ///     The SRP "salt" value for the client to use during authentication.
    /// </summary>
    [Required]
    [StringLength(512)]
    public string Salt { get; set; }

    /// <summary>
    ///     The HoN specific "salt" value used in the password hashing algorithm by the client.
    ///     Referred to by the HoN client as `salt2`.
    /// </summary>
    [Required]
    [StringLength(22)]
    public string PasswordSalt { get; set; }

    /// <summary>
    ///     Hashed password that is used for authentication.
    /// </summary>
    [Required]
    [StringLength(64)]
    public string HashedPassword { get; set; }

    /// <summary>
    ///     Number of gold coins that the user has.
    /// </summary>
    public int GoldCoins { get; set; }

    /// <summary>
    ///     Number of silver coins that the user has.
    /// </summary>
    public int SilverCoins { get; set; }

    /// <summary>
    ///     Number of Plinko tickets that the user has.
    /// </summary>
    public int PlinkoTickets { get; set; }

    /// <summary>
    ///     Collection of content ids that the user has unlocked.
    /// </summary>
    [Required]
    public ICollection<string> UnlockedUpgradeCodes { get; set; }
}
