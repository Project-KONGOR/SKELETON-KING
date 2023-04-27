namespace ZORGATH;

public class SeasonShortSummary
{
    public SeasonShortSummary(int wins, int losses, int winStreak, int currentLevel)
    {
        Wins = wins;
        Losses = losses;
        WinStreak = winStreak;
        CurrentLevel = currentLevel;
    }

    [PhpProperty("wins")]
    public readonly int Wins;
    [PhpProperty("losses")]
    public readonly int Losses;
    [PhpProperty("win_streak")]
    public readonly int WinStreak;
    [PhpProperty("current_level")]
    public readonly int CurrentLevel;
}

public class ShowSimpleStatsResponse
{
    public ShowSimpleStatsResponse(string nickname, int level, int levelExperience, int numberOfHeroesOwned, int numberOfAvatarsOwned, int totalMatchesPlayed, int numberOfMVPAwards, ICollection<string> selectedUpgrades, int accountId, int seasonId, SeasonShortSummary seasonNormal, SeasonShortSummary seasonCasual, List<string> awardTop4Names, List<int> awardTop4Nums)
    {
        Nickname = nickname;
        Level = level;
        LevelExperience = levelExperience;
        NumberOfHeroesOwned = numberOfHeroesOwned;
        NumberOfAvatarsOwned = numberOfAvatarsOwned;
        TotalMatchesPlayed = totalMatchesPlayed;
        NumberOfMVPAwards = numberOfMVPAwards;
        SelectedUpgrades = selectedUpgrades;
        AccountId = accountId;
        SeasonId = seasonId;
        SeasonNormal = seasonNormal;
        SeasonCasual = seasonCasual;
        AwardTop4Names = awardTop4Names;
        AwardTop4Nums = awardTop4Nums;
    }

    [PhpProperty("nickname")]
    public readonly string Nickname;
    [PhpProperty("level")]
    public readonly int Level;
    [PhpProperty("level_exp")]
    public readonly int LevelExperience;
    [PhpProperty("hero_num")]
    public readonly int NumberOfHeroesOwned;
    [PhpProperty("avatar_num")]
    public readonly int NumberOfAvatarsOwned;
    [PhpProperty("total_played")]
    public readonly int TotalMatchesPlayed;
    [PhpProperty("mvp_num")]
    public readonly int NumberOfMVPAwards;
    [PhpProperty("selected_upgrades")]
    public readonly ICollection<string> SelectedUpgrades;
    [PhpProperty("account_id")]
    public readonly int AccountId;
    [PhpProperty("season_id")]
    public readonly int SeasonId;
    [PhpProperty("season_normal")]
    public readonly SeasonShortSummary SeasonNormal;
    [PhpProperty("season_casual")]
    public readonly SeasonShortSummary SeasonCasual;
    [PhpProperty("award_top4_name")]
    public readonly List<string> AwardTop4Names;
    [PhpProperty("award_top4_num")]
    public readonly List<int> AwardTop4Nums;
}
