namespace ZORGATH;

public record ShowSimpleStatsData(
    int TotalLevel,
    int TotalExperience,
    int NumberOfHeroesOwned,
    int TotalMatchesPlayed,
    int NumberOfMVPAwards,
    ICollection<string> UnlockedUpgradeCodes,
    ICollection<string> SelectedUpgradeCodes,
    int AccountId,
    int SeasonId,
    SeasonShortSummary SeasonNormal,
    SeasonShortSummary SeasonCasual);

public class ShowSimpleStatsHandler : IClientRequesterHandler
{
    private const int NumberOfHeroesTheGameHas = 1239;

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
                /* TotalMatchesPlayed: */ 0,
                /* NumberOfMVPAwards: */ 0,
                /* SelectedUpgrades: */ account.SelectedUpgradeCodes,
                /* UnlockedUpgradeCodes: */ account.User.UnlockedUpgradeCodes,
                /* AccountId: */ account.AccountId,
                /* SeasonId: */ CurrentSeason,
                /* SeasonNormal: */ new SeasonShortSummary(0, 0, 0, 0),
                /* SeasonCasual: */ new SeasonShortSummary(0, 0, 0, 0))
            )
            .FirstOrDefaultAsync();

        if (data is null)
        {
            return new NotFoundResult();
        }

        // TODO: include awards once we have stats.
        ShowSimpleStatsResponse showSimpleStatsResponse = new(
            nickname,
            data.TotalLevel,
            data.TotalExperience,
            data.NumberOfHeroesOwned,
            data.UnlockedUpgradeCodes.Count(upgrade => upgrade.StartsWith("aa.")),
            data.TotalMatchesPlayed,
            data.NumberOfMVPAwards,
            data.SelectedUpgradeCodes,
            data.AccountId,
            data.SeasonId,
            data.SeasonNormal,
            data.SeasonCasual,
            new(),
            new()
        );
        return new OkObjectResult(PHP.Serialize(showSimpleStatsResponse));
    }
}
