namespace PUZZLEBOX;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class PlayerSeasonStats
{
    public const int NumPlacementMatches = 6;
    public float Rating { get; set; } = 1500;
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Concedes { get; set; }
    public int ConcedeVotes { get; set; }
    public int Buybacks { get; set; }
    public int TimesDisconnected { get; set; }
    public int TimesKicked { get; set; }
    public int HeroKills { get; set; }
    public int HeroDamage { get; set; }
    public int HeroExp { get; set; }
    public int HeroKillsGold { get; set; }
    public int HeroAssists { get; set; }
    public int Deaths { get; set; }
    public int GoldLost2Death { get; set; }
    public int SecsDead { get; set; }
    public int TeamCreepKills { get; set; }
    public int TeamCreepDmg { get; set; }
    public int TeamCreepExp { get; set; }
    public int TeamCreepGold { get; set; }
    public int NeutralCreepKills { get; set; }
    public int NeutralCreepDmg { get; set; }
    public int NeutralCreepExp { get; set; }
    public int NeutralCreepGold { get; set; }
    public int BDmg { get; set; }
    public int BDmgExp { get; set; }
    public int Razed { get; set; }
    public int BGold { get; set; }
    public int Denies { get; set; }
    public int ExpDenies { get; set; }
    public int Gold { get; set; }
    public int GoldSpent { get; set; }
    public int Exp { get; set; }
    public int Actions { get; set; }
    public int Secs { get; set; }
    public int Consumables { get; set; }
    public int Wards { get; set; }
    public int EmPlayed { get; set; }
    public int Level { get; set; }
    public int LevelExp { get; set; }
    public int MinExp { get; set; }
    public int MaxExp { get; set; }
    public int TimeEarningExp { get; set; }
    public int Bloodlust { get; set; }
    public int DoubleKill { get; set; }
    public int TrippleKill { get; set; }
    public int QuadKill { get; set; }
    public int Annihilation { get; set; }
    public int Ks3 { get; set; }
    public int Ks4 { get; set; }
    public int Ks5 { get; set; }
    public int Ks6 { get; set; }
    public int Ks7 { get; set; }
    public int Ks8 { get; set; }
    public int Ks9 { get; set; }
    public int Ks10 { get; set; }
    public int Ks15 { get; set; }
    public int Smackdown { get; set; }
    public int Humiliation { get; set; }
    public int Nemesis { get; set; }
    public int Retribution { get; set; }
    public int WinStreak { get; set; }

    [Required]
    public string SerializedMatchIds { get; set; } = "";

    [Required]
    // This only needs to be in Ranked non-Casual?
    public PlayerAwardSummary PlayerAwardSummary { get; set; } = new();

    [Required]
    public string SerializedHeroUsage { get; set; } = "";

    [Required]
    [StringLength(NumPlacementMatches)]
    public string PlacementMatchesDetails { get; set; } = "";
}

public class PlayerSeasonStatsRanked : PlayerSeasonStats
{
    [Key]
    public int AccountId { get; set; }
}

public class PlayerSeasonStatsRankedCasual : PlayerSeasonStats
{
    [Key]
    public int AccountId { get; set; }
}

public class PlayerSeasonStatsPublic : PlayerSeasonStats
{
    [Key]
    public int AccountId { get; set; }
}

public class PlayerSeasonStatsMidWars : PlayerSeasonStats
{
    [Key]
    public int AccountId { get; set; }
}

public class PlayerSeasonStatsGrimmsCrossing : PlayerSeasonStats
{
    [Key]
    public int AccountId { get; set; }
}

public class TeamSeasonStatsGrimmsCrossing : PlayerSeasonStats
{
    [Key]
    public string AccountIds { get; set; }
}
