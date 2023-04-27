﻿namespace ZORGATH;

public class SrpAuthHandler : IClientRequesterHandler
{
    private readonly ConcurrentDictionary<string, SrpAuthSessionData> _srpAuthSessions;
    private readonly SecInfo _secInfo;

    public SrpAuthHandler(ConcurrentDictionary<string, SrpAuthSessionData> srpAuthSessions, SecInfo secInfo)
    {
        _srpAuthSessions = srpAuthSessions;
        _secInfo = secInfo;
    }

    public async Task<IActionResult> HandleRequest(ControllerContext controllerContext, Dictionary<string, string> formData)
    {
        string login = formData["login"];
        if (!_srpAuthSessions.Remove(login, out SrpAuthSessionData? srpAuthSessionData))
        {
            return new NotFoundObjectResult(PHP.Serialize(new AuthFailedResponse(AuthFailureReason.InvalidSession)));
        }

        // Check if the proof matches, which means that the password also matches.
        string? proof = FinalizeSRPAuthentication(login, srpAuthSessionData, formData["proof"]);
        if (proof is null)
        {
            return new UnauthorizedObjectResult(PHP.Serialize(new AuthFailedResponse(AuthFailureReason.IncorrectPassword)));
        }

        // Load data for the account.
        // TODO: load that as part of preAuth to avoid an extra query?
        using BountyContext bountyContext = controllerContext.HttpContext.RequestServices.GetRequiredService<BountyContext>();
        AccountDetails? accountDetails = await bountyContext.Accounts
            .Where(account => account.Name == login)
            .Select(account => new AccountDetails(
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
            ))
            .FirstOrDefaultAsync();
        if (accountDetails is null)
        {
            return new NotFoundObjectResult(PHP.Serialize(new AuthFailedResponse(AuthFailureReason.AccountNotFound)));
        }
        await accountDetails.Load(bountyContext);

        // Generate a new cookie for the Account.
        string cookie = Guid.NewGuid().ToString("N");
        await bountyContext.Accounts
            .Where(account => account.AccountId == accountDetails.AccountId)
            .ExecuteUpdateAsync(update => update.SetProperty(account => account.Cookie, cookie));

        string clientIpAddress = controllerContext.HttpContext.Connection.RemoteIpAddress.ToString();
        long hostTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        // TODO: update these once we support custom account icons and the chatserver.
        string chatServerUrl = "localhost";
        string icbUrl = "kongor.online";
        SrpAuthResponse response = new(accountDetails, cookie, clientIpAddress, hostTime, chatServerUrl, icbUrl, proof, _secInfo);
        return new OkObjectResult(PHP.Serialize(response));
    }

    /// <summary>
    ///     Finalizes an SRP auth by computing the `proof`.
    /// </summary>
    /// 
    /// <param name="authenticationSessionData">
    ///     The current SRP authentication data for the auth session. These values are used to
    ///     complete the SRP auth calculations.
    /// </param>
    /// 
    /// <returns>
    ///     Returns the `proof` string from the SRP auth session, or `null` if there was an error.
    /// </returns>
    private static string? FinalizeSRPAuthentication(string login, SrpAuthSessionData autSessionData, string clientProof)
    {
        SrpParameters parameters = SrpParameters.Create<SHA256>(SrpAuthSessionData.N, SrpAuthSessionData.g);

        // NOTE: HoN SRP requires a padded `g` (`parameters.Generator`) value for it's final `M`
        // calculation.  This is not handled by the SRP library since the rfc5054 specification is unclear as to 
        // whether this should be done. But it's required for HoN's SRP.
        parameters.Generator = parameters.Pad(parameters.Generator);

        SrpServer srpServer = new(parameters);
        try
        {
            SrpSession serverSession = srpServer.DeriveSession(autSessionData.ServerEphemeral.Secret, autSessionData.ClientPublicEphemeral, autSessionData.Salt, login, autSessionData.Verifier, clientProof);
            return serverSession.Proof;
        }
        catch
        {
            return null;
        }
    }
}
