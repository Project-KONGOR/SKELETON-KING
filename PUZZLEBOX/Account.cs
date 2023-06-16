namespace PUZZLEBOX;

/// <summary>
///     An Entity Framework object representing a playable Account.
/// </summary>
[Index(nameof(Name), IsUnique = true)]
[Index(nameof(Cookie), IsUnique = true)]
public class Account
{
    public Account(string name, ICollection<string>? ignoredList = null, ICollection<string>? selectedUpgradeCodes = null, ICollection<string>? autoConnectChatChannels = null)
    {
        Name = name;
        IgnoredList = ignoredList ?? Array.Empty<string>();
        SelectedUpgradeCodes = selectedUpgradeCodes ?? Array.Empty<string>();
        AutoConnectChatChannels = autoConnectChatChannels ?? Array.Empty<string>();
    }

    [Key]
    public int AccountId { get; set; }

    [Required]
    [MaxLength(15)]
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
    public ICollection<string> AutoConnectChatChannels;

    [Required]
    public ICollection<string> IgnoredList { get; set; }

    [Required]
    public ICollection<string> SelectedUpgradeCodes;

    [StringLength(32)]
    public string? Cookie { get; set; }

    [Required]
    public PlayerSeasonStatsRanked PlayerSeasonStatsRanked { get; set; }

    [Required]
    public PlayerSeasonStatsRankedCasual PlayerSeasonStatsRankedCasual { get; set; }

    [Required]
    public PlayerSeasonStatsPublic PlayerSeasonStatsPublic { get; set; }

    [Required]
    public PlayerSeasonStatsMidWars PlayerSeasonStatsMidWars { get; set; }

    public PlayerSeasonStatsGrimmsCrossing? PlayerSeasonStatsGrimmsCrossing { get; set; }
}
