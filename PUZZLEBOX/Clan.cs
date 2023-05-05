namespace PUZZLEBOX;

[Index(nameof(Name), IsUnique = true)]
[Index(nameof(Tag), IsUnique = true)]
public class Clan
{
    public enum Tier
    {
        None,
        Member,
        Officer,
        Leader
    }

    public static string GetClanTierName(Tier tier)
    {
        return tier switch
        {
            Tier.Member => "Member",
            Tier.Officer => "Officer",
            Tier.Leader => "Leader",
            _ => "None",
        };
    }

    public int ClanId { get; set; }

    [Required]
    [StringLength(25)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(4)]
    public string Tag { get; set; } = null!;

    [Required]
    public DateTime TimestampCreated { get; set; }
}
