namespace ZORGATH;

public record AccountInfo(string Salt, string PasswordSalt, string HashedPassword);

/// <summary>
/// client_requester.php?f=preAuth
/// Performs the first half of the SRP key exchange. Stores the intermediate results in a ConcurrentDictionary for the
/// second half of the SRP to validate the exchange.
/// </summary>
public class PreAuthHandler : IClientRequesterHandler
{
    private readonly ConcurrentDictionary<string, SrpAuthSessionData> _srpAuthSessions;

    public PreAuthHandler(ConcurrentDictionary<string, SrpAuthSessionData> srpAuthSessions)
    {
        _srpAuthSessions = srpAuthSessions;
    }

    public async Task<IActionResult> HandleRequest(ControllerContext controllerContext, Dictionary<string, string> formData)
    {
        using BountyContext bountyContext = controllerContext.HttpContext.RequestServices.GetRequiredService<BountyContext>();
        string login = formData["login"];

        SrpAuthSessionData? srpAuthSessionData = await bountyContext.Accounts
            .Where(account => account.Name == login)
            .Select(account => new SrpAuthSessionData(
                login,
                formData["A"],
                account.User.Salt,
                account.User.PasswordSalt,
                account.User.HashedPassword,
                new AccountDetails(
                    account.AccountId,
                    account.Name,
                    account.AccountType,
                    account.SelectedUpgradeCodes,
                    account.AutoConnectChatChannels,
                    account.IgnoredList,

                    // TODO: query stats too.
                    new AccountStats(
                        /* level: */ 0,
                        /* levelExp: */ 0,
                        /* psr: */ 1500,
                        /* normalRankedGamesMMR: */ 1500,
                        /* casualModeMMR: */ 1500,
                        /* publicGamesPlayed: */ 0,
                        /* normalRankedGamesPlayed: */ 0,
                        /* casualModeGamesPlayed: */ 0,
                        /* midWarsGamesPlayed: */ 0,
                        /* allOtherGamesPlayed: */ 0,
                        /* publicGameDisconnects: */ 0,
                        /* normalRankedGameDisconnects: */ 0,
                        /* casualModeDisconnects: */ 0,
                        /* midWarsTimesDisconnected: */ 0,
                        /* allOtherGameDisconnects: */ 0),

                    // Clan information.
                    account.Clan!.ClanId,
                    account.Clan!.Name,
                    account.Clan!.Tag,
                    account.ClanTier,

                    // TODO: CloudStorage.
                    /* useCloud: */ false,
                    /* cloudAutoUpload: */ false,

                    // User-specific information.
                    account.User.Id,
                    account.User.Email!,
                    account.User.GoldCoins,
                    account.User.SilverCoins,
                    account.User.UnlockedUpgradeCodes
                )))
            .FirstOrDefaultAsync();
        if (srpAuthSessionData is null)
        {
            return new NotFoundObjectResult(PHP.Serialize(new AuthFailedResponse(AuthFailureReason.AccountNotFound)));
        }

        _srpAuthSessions[login] = srpAuthSessionData;

        PreAuthResponse response = new(srpAuthSessionData.Salt, srpAuthSessionData.PasswordSalt, srpAuthSessionData.ServerEphemeral.Public);
        return new OkObjectResult(PHP.Serialize(response));
    }
}
