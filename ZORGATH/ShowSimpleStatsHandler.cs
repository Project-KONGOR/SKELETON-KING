namespace ZORGATH;

class CombinedPlayerAwardSummary
{
    public readonly int MVP;
    public readonly List<string> Top4Names;
    public readonly List<int> Top4Nums;

    CombinedPlayerAwardSummary(int mvp, List<string> top4Names, List<int> top4Nums)
    {
        MVP = mvp;
        Top4Names = top4Names;
        Top4Nums = top4Nums;
    }

    public static CombinedPlayerAwardSummary AddUp(PlayerAwardSummary pas1, PlayerAwardSummary pas2, PlayerAwardSummary pas3, PlayerAwardSummary pas4)
    {
        PlayerAwardSummary playerAwardSummary = new PlayerAwardSummary()
        {
            TopAnnihilations = pas1.TopAnnihilations + pas2.TopAnnihilations + pas3.TopAnnihilations + pas4.TopAnnihilations,
            MostQuadKills = pas1.MostQuadKills + pas2.MostQuadKills + pas3.MostQuadKills + pas4.MostQuadKills,
            BestKillStreak = pas1.BestKillStreak + pas2.BestKillStreak + pas3.BestKillStreak + pas4.BestKillStreak,
            MostSmackdowns = pas1.MostSmackdowns + pas2.MostSmackdowns + pas3.MostSmackdowns + pas4.MostSmackdowns,
            MostKills = pas1.MostKills + pas2.MostKills + pas3.MostKills + pas4.MostKills,
            MostAssists = pas1.MostAssists + pas2.MostAssists + pas3.MostAssists + pas4.MostAssists,
            LeastDeaths = pas1.LeastDeaths + pas2.LeastDeaths + pas3.LeastDeaths + pas4.LeastDeaths,
            TopSiegeDamage = pas1.TopSiegeDamage + pas2.TopSiegeDamage + pas3.TopSiegeDamage + pas4.TopSiegeDamage,
            MostWardsKilled = pas1.MostWardsKilled + pas2.MostWardsKilled + pas3.MostWardsKilled + pas4.MostWardsKilled,
            TopHeroDamage = pas1.TopHeroDamage + pas2.TopHeroDamage + pas3.TopHeroDamage + pas4.TopHeroDamage,
            TopCreepScore = pas1.TopCreepScore + pas2.TopCreepScore + pas3.TopCreepScore + pas4.TopCreepScore,
            MVP = pas1.MVP + pas2.MVP + pas3.MVP + pas4.MVP,
        };

        List<KeyValuePair<int, string>> awardToCount = new();
        awardToCount.Add(new KeyValuePair<int, string>(playerAwardSummary.TopAnnihilations, "awd_mann"));
        awardToCount.Add(new KeyValuePair<int, string>(playerAwardSummary.MostQuadKills, "awd_mqk"));
        awardToCount.Add(new KeyValuePair<int, string>(playerAwardSummary.BestKillStreak, "awd_lgks"));
        awardToCount.Add(new KeyValuePair<int, string>(playerAwardSummary.MostSmackdowns, "awd_msd"));
        awardToCount.Add(new KeyValuePair<int, string>(playerAwardSummary.MostKills, "awd_mkill"));
        awardToCount.Add(new KeyValuePair<int, string>(playerAwardSummary.MostAssists, "awd_masst"));
        awardToCount.Add(new KeyValuePair<int, string>(playerAwardSummary.LeastDeaths, "awd_ledth"));
        awardToCount.Add(new KeyValuePair<int, string>(playerAwardSummary.TopSiegeDamage, "awd_mbdmg"));
        awardToCount.Add(new KeyValuePair<int, string>(playerAwardSummary.MostWardsKilled, "awd_mwk"));
        awardToCount.Add(new KeyValuePair<int, string>(playerAwardSummary.TopHeroDamage, "awd_mhdd"));
        awardToCount.Add(new KeyValuePair<int, string>(playerAwardSummary.TopCreepScore, "awd_hcs"));

        awardToCount.Sort((a, b) => -a.Key.CompareTo(b.Key));
        return new CombinedPlayerAwardSummary(mvp: playerAwardSummary.MVP, top4Names: awardToCount.Select(a => a.Value).Take(4).ToList(), top4Nums: awardToCount.Select(a => a.Key).Take(4).ToList());
    }
}

record ShowSimpleStatsData(
    int TotalLevel,
    int TotalExperience,
    int NumberOfHeroesOwned,
    int TotalMatchesPlayed,
    CombinedPlayerAwardSummary CombinedPlayerAwardSummary,
    ICollection<string> SelectedUpgradeCodes,
    ICollection<string> UnlockedUpgradeCodes,
    int AccountId,
    int SeasonId,
    SeasonShortSummary SeasonNormal,
    SeasonShortSummary SeasonCasual);

public class ShowSimpleStatsHandler : IClientRequesterHandler
{
    private const int NumberOfHeroesTheGameHas = 139;

    // In Heroes of Newerth, there is a slightly different logic when computing stats for Seasons <= 6 and Seasons > 6.
    // Additionally, season id must be < 1000. 22 was chosen somewhat arbitrarily and primarily because revival started
    // in the year 2022, but anything > 6 should work just as well.
    private const int CurrentSeason = 22;

    public async Task<IActionResult> HandleRequest(ControllerContext controllerContext, Dictionary<string, string> formData)
    {
        using var bountyContext = controllerContext.HttpContext.RequestServices.GetRequiredService<BountyContext>();
        string cookie = formData["cookie"];

        // Validate cookie.
        if (!await bountyContext.Accounts.AnyAsync(a => a.Cookie == cookie))
        {
            // Invalid cookie.
            return new UnauthorizedResult();
        }

        string nickname = formData["nickname"];
        ShowSimpleStatsData? data = await bountyContext.Accounts
            .Where(account => account.Name == nickname)
            .Select(account => new ShowSimpleStatsData(
                /* TotalLevel: */ 0,
                /* TotalExperience: */ 0,
                /* NumberOfHeroesOwned: */ NumberOfHeroesTheGameHas,
                /* TotalMatchesPlayed: */ 
                    // Public
                    account.PlayerSeasonStatsPublic.Wins + account.PlayerSeasonStatsPublic.Losses +
                    // Ranked
                    account.PlayerSeasonStatsRanked.Wins + account.PlayerSeasonStatsRanked.Losses +
                    // Casual
                    account.PlayerSeasonStatsRankedCasual.Wins + account.PlayerSeasonStatsRankedCasual.Losses +
                    // Mid Wars
                    account.PlayerSeasonStatsMidWars.Wins + account.PlayerSeasonStatsMidWars.Losses,
                /* PlayerAwardSummary: */ CombinedPlayerAwardSummary.AddUp(account.PlayerSeasonStatsPublic.PlayerAwardSummary, account.PlayerSeasonStatsRanked.PlayerAwardSummary, account.PlayerSeasonStatsRankedCasual.PlayerAwardSummary, account.PlayerSeasonStatsMidWars.PlayerAwardSummary),
                /* SelectedUpgrades: */ account.SelectedUpgradeCodes,
                /* UnlockedUpgradeCodes: */ account.User.UnlockedUpgradeCodes,
                /* AccountId: */ account.AccountId,
                /* SeasonId: */ CurrentSeason,
                /* SeasonNormal: */ new SeasonShortSummary(account.PlayerSeasonStatsRanked.Wins + account.PlayerSeasonStatsMidWars.Wins, account.PlayerSeasonStatsRanked.Losses + account.PlayerSeasonStatsMidWars.Losses, 0, 0),
                /* SeasonCasual: */ new SeasonShortSummary(account.PlayerSeasonStatsRankedCasual.Wins, account.PlayerSeasonStatsRankedCasual.Losses, 0, 0))
            )
            .FirstOrDefaultAsync();

        if (data is null)
        {
            return new NotFoundResult();
        }

        ShowSimpleStatsResponse showSimpleStatsResponse = new(
            nickname,
            data.TotalLevel,
            data.TotalExperience,
            data.NumberOfHeroesOwned,
            data.UnlockedUpgradeCodes.Count(upgrade => upgrade.StartsWith("aa.")),
            data.TotalMatchesPlayed,
            data.CombinedPlayerAwardSummary.MVP,
            data.SelectedUpgradeCodes,
            data.AccountId,
            data.SeasonId,
            data.SeasonNormal,
            data.SeasonCasual,
            data.CombinedPlayerAwardSummary.Top4Names,
            data.CombinedPlayerAwardSummary.Top4Nums
        );
        return new OkObjectResult(PHP.Serialize(showSimpleStatsResponse));
    }
}
