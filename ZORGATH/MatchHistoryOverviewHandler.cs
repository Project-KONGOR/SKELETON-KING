namespace ZORGATH;

/// <summary>
///     Handler for "match_history_overview" request that fetches last "num" games played by a given player.
/// </summary>
public class MatchHistoryOverviewHandler : IClientRequesterHandler
{
    public async Task<IActionResult> HandleRequest(ControllerContext controllerContext, Dictionary<string, string> formData)
    {
        // Check if the cookie is correct.
        string cookie = formData["cookie"];
        using BountyContext bountyContext = controllerContext.HttpContext.RequestServices.GetRequiredService<BountyContext>();
        if (!await bountyContext.Accounts.AnyAsync(account => account.Cookie == cookie))
        {
            // Access denies due to invalid cookie.
            return new UnauthorizedResult();
        }

        // Nickname of the player.
        string nickname = formData["nickname"];

        // Identifies the type of the matches that we are interested in.
        string table = formData["table"];

        // Grab the match IDs according to the game type.
        string? serializedMatchIds = table switch
        {
            "campaign" => await bountyContext.Accounts
                .Where(account => account.Name == nickname)
                .Select(a => a.PlayerSeasonStatsRanked.SerializedMatchIds)
                .FirstOrDefaultAsync(),
            "campaign_casual" => await bountyContext.Accounts
                .Where(account => account.Name == nickname)
                .Select(a => a.PlayerSeasonStatsRankedCasual.SerializedMatchIds)
                .FirstOrDefaultAsync(),
            "player" => await bountyContext.Accounts
                .Where(account => account.Name == nickname)
                .Select(a => a.PlayerSeasonStatsPublic.SerializedMatchIds)
                .FirstOrDefaultAsync(),
            "midwars" => await bountyContext.Accounts
                .Where(account => account.Name == nickname)
                .Select(a => a.PlayerSeasonStatsMidWars.SerializedMatchIds)
                .FirstOrDefaultAsync(),
            _ => null
        };
        if (serializedMatchIds == null)
        {
            // Account was not found.
            return new NotFoundResult();
        }

        int numberOfMatchesToRetrieve = int.Parse(formData["num"]);
        return new OkObjectResult(PHP.Serialize(await GetMatchHistoryOverview(bountyContext, nickname, serializedMatchIds, numberOfMatchesToRetrieve)));
    }

    /// <summary>
    ///     Given a serialized list of match ids, returns information about the last 100 matches.
    /// </summary>
    /// <param name="bountyContext"></param>
    /// <param name="nickname"></param>
    /// <param name="serializedMatchIds"></param>
    /// <param name="numberOfMatchesToRetrieve"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, string>> GetMatchHistoryOverview(BountyContext bountyContext, string nickname, string serializedMatchIds, int numberOfMatchesToRetrieve)
    {
        List<string> allMatchIds = serializedMatchIds.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();
        IEnumerable<int> recentMatchIds = allMatchIds.TakeLast(numberOfMatchesToRetrieve).Reverse().Select(matchId => int.Parse(matchId));

        // This would match [CLAN]nickname
        string nicknameSuffix = $"]{nickname}";

        // Get the list of comma-separated stats.
        List<string> playerMatchResultsList = await bountyContext.PlayerMatchResults
            .Where(results => recentMatchIds.Contains(results.match_id) && (results.nickname == nickname || results.nickname.EndsWith(nicknameSuffix)))
            .OrderByDescending(results => results.match_id)
            .Select(results => string.Join(',',
                results.match_id,
                results.wins,
                results.team,
                results.herokills,
                results.deaths,
                results.heroassists,
                results.hero_id,
                results.secs,
                results.map,
                results.mdt,
                results.cli_name))
            .ToListAsync();

        // Turn it into a Dictionary with keys ranging from m0 to m99.
        Dictionary<string, string> matchHistoryOverview = new();
        for (int i = 0; i < playerMatchResultsList.Count; i++)
        {
            matchHistoryOverview.Add("m" + i, playerMatchResultsList[i]);
        }

        return matchHistoryOverview;
    }
}
