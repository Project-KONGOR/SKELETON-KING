using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ShowSimpleStatsHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<IActionResult> HandleRequest(ControllerContext controllerContext, Dictionary<string, string> formData)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();
        using var bountyContext = serviceScope.ServiceProvider.GetService<BountyContext>()!;
        string nickname = formData["nickname"];

        ShowSimpleStatsData? data = await bountyContext.Accounts
            .Where(account => account.Name == nickname)
            .Select(account => new ShowSimpleStatsData(
                /* TotalLevel: */ 0,
                /* TotalExperience: */ 0,
                /* NumberOfHeroesOwned: */ 139,
                /* TotalMatchesPlayed: */ 0,
                /* NumberOfMVPAwards: */ 0,
                /* SelectedUpgrades: */ account.SelectedUpgradeCodes,
                /* UnlockedUpgradeCodes: */ account.User.UnlockedUpgradeCodes,
                /* AccountId: */ account.AccountId,
                /* SeasonId: */ 22,
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
