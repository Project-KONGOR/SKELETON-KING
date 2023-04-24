using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ZORGATH;

/// <summary>
///     An Entity Framework object representing an owner of one or more Accounts. Includes information shared by all
///     accounts, such as password-related information and any of the content they have unlocked.
/// </summary>
[Index(nameof(UserName), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class ElementUser : IdentityUser
{
    public ElementUser(string salt, string passwordSalt, string hashedPassword, string email, int points, int mmPoints, int tickets, ICollection<string> unlockedUpgradeCodes)
        : this(salt, passwordSalt, hashedPassword, email, unlockedUpgradeCodes)
    {
        Points = points;
        MMPoints = mmPoints;
        Tickets = tickets;
    }

    public ElementUser(string salt, string passwordSalt, string hashedPassword, string email, ICollection<string> unlockedUpgradeCodes)
    {
        Salt = salt;
        PasswordSalt = passwordSalt;
        HashedPassword = hashedPassword;
        UnlockedUpgradeCodes = unlockedUpgradeCodes;

        // Base Properties
        Email = email;
        EmailConfirmed = true;
    }

    [Required]
    public string Salt { get; set; }

    [Required]
    public string PasswordSalt { get; set; }
    [Required]
    public string HashedPassword { get; set; }

    public int Points { get; set; }

    public int MMPoints { get; set; }

    public int Tickets { get; set; }

    [Required]
    public ICollection<string> UnlockedUpgradeCodes { get; set; }
}
