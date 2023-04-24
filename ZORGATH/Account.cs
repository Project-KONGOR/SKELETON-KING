using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ZORGATH;

/// <summary>
///     An Entity Framework object representing a playable Account.
/// </summary>
[Index(nameof(Name), IsUnique = true)]
[Index(nameof(Cookie), IsUnique = true)]
public class Account
{
    [Key]
    public int AccountId { get; set; }

    [Required]
    [StringLength(20)]
    public string Name { get; set; } = null!;

    [Required]
    // This is a transition property, automatically initialized by EF. Never null.
    public ElementUser User { get; set; } = null!;

    public Clan? Clan { get; set; }

    public Clan.Tier ClanTier { get; set; }

    public CloudStorage? CloudStorage { get; set; }

    [Required]
    public DateTime TimestampCreated { get; set; }

    [Required]
    public DateTime LastActivity { get; set; }

    public AccountType AccountType { get; set; } = AccountType.Legacy;

    [Required]
    public ICollection<string> AutoConnectChatChannels = null!;

    [Required]
    public ICollection<string> IgnoredList { get; set; } = null!;

    [Required]
    public ICollection<string> SelectedUpgradeCodes = null!;

    [StringLength(32)]
    public string? Cookie { get; set; }
}
