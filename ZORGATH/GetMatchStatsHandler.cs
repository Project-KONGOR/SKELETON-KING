namespace ZORGATH;

/// <summary>
///     Handler for "get_match_stats" request.
/// </summary>
public class GetMatchStatsHandler : IClientRequesterHandler
{
    private string _replayServerUrl;
    private record AccountIdWithSelectedUpgradeCodes(int AccountId, ICollection<string> SelectedUpgradeCodes);

    public GetMatchStatsHandler(string replayServerUrl)
    {
        _replayServerUrl = replayServerUrl;
    }

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

        int matchId = int.Parse(formData["match_id"]);
        MatchResults? matchResults = await bountyContext.MatchResults.FirstOrDefaultAsync(matchResults => matchResults.match_id == matchId);
        if (matchResults is null)
        {
            // Match results not found.
            return new NotFoundResult();
        }

        List<PlayerMatchResults> playerMatchResults = await bountyContext.PlayerMatchResults.Where(playerMatchResults => playerMatchResults.match_id == matchId).ToListAsync();
        MatchSummary matchSummary = new(matchResults, winningTeam: DetermineWinningTeam(playerMatchResults), _replayServerUrl);
        Dictionary<string, object> response = new()
        {
            ["0"] = true, // mandatory, indicates success.
            ["match_summ"] = new Dictionary<int, object> { { matchId, matchSummary } },
            ["match_player_stats"] = new Dictionary<int, object> { { matchId, playerMatchResults } }

            // Other possible values:
            // ["rewards"] = ...
            // ["referrers"] = ...
            // ["can_be_referred"] = ...
            // ["con_reward"] = ...
            // ["match_mastery"] = ...
            // ["season_system"] = ...
            // ["mmpoints"] = ...
            // ["selected_upgrades"] = ...
        };

        // inventory is null if the result is not submitted or the game is remade.
        if (matchResults.inventory != null)
        {
            response["inventory"] = new List<Dictionary<int, Dictionary<string, string>>> { System.Text.Json.JsonSerializer.Deserialize<Dictionary<int, Dictionary<string, string>>>(matchResults.inventory) ?? new Dictionary<int, Dictionary<string, string>>() };
        }

        return new OkObjectResult(PhpSerialization.Serialize(response));
    }

    /// <summary>
    ///     Determine the winning team based on whether the players won or lost.
    ///     1 corresponds to Legion, 2 corresponds to Hellbourne, 0 if remade (or error).
    /// </summary>
    private int DetermineWinningTeam(List<PlayerMatchResults> playerMatchResults)
    {
        foreach (PlayerMatchResults results in playerMatchResults)
        {
            if (results.wins == 1)
            {
                return results.team;
            }
        }

        // Neither (the game was remade or not properly recorded).
        return 0;
    }
}
