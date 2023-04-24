using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SecureRemotePassword;
using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace ZORGATH;

public class SrpAuthHandler : IClientRequesterHandler
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ConcurrentDictionary<string, SrpAuthSessionData> _srpAuthSessions;

    public SrpAuthHandler(IServiceScopeFactory serviceScopeFactory, ConcurrentDictionary<string, SrpAuthSessionData> srpAuthSessions)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _srpAuthSessions = srpAuthSessions;
    }
    
    public async Task<IActionResult> HandleRequest(ControllerContext controllerContext, Dictionary<string, string> formData)
    {
        string login = formData["login"];
        if (!_srpAuthSessions.Remove(login, out SrpAuthSessionData? srpAuthSessionData))
        {
            return new NotFoundObjectResult(PHP.Serialize(new AuthFailedResponse(AuthFailureReason.InvalidSession)));
        }

        string clientProof = formData["proof"];
        using var scope = _serviceScopeFactory.CreateScope();
        using var bountyContext = scope.ServiceProvider.GetService<BountyContext>()!;

        AccountDetailsForLogin? accountDetails = await bountyContext.Accounts
            .Where(account => account.Name == login)
            .Select(account => new AccountDetailsForLogin(
                account.User.Id,
                account.User.UserName!,
                account,
                account.User.Email!,
                account.User.Points,
                account.User.MMPoints,
                account.User.UnlockedUpgradeCodes,
                account.Clan!.ClanId,
                account.Clan!.Name,
                account.Clan!.Tag,
                account.CloudStorage!.UseCloud,
                account.CloudStorage!.CloudAutoUpload,
                account.CloudStorage!.FileModifyTime,

                // TODO: populate stats.
                /* publicRating: */ 1500,
                /* publicGamesWon: */ 0,
                /* publicGamesLost: */ 0,
                /* publicTimesDisconnected: */ 0,

                /* midWarsRating: */ 1500,
                /* midWarsGamesWon: */ 0,
                /* midWarsGamesLost: */ 0,
                /* midWarsTimesDisconnected: */ 0,
                /* midWarsPlacementsDone: */ 0,

                /* conNormalRating: */ 1500,
                /* conNormalGamesWon: */ 0,
                /* conNormalGamesLost: */ 0,
                /* conNormalTimesDisconnected: */ 0,
                /* conNormalPlacementsDone: */ 0,

                /* conCasualRating: */ 1500,
                /* conCasualGamesWon: */ 0,
                /* conCasualGamesLost: */ 0,
                /* conCasualTimesDisconnected: */ 0,
                /* conCasualPlacementsDone: */ 0,

                /* tournamentGamesWon: */ 0,
                /* tournamentGamesLost: */ 0,
                /* tournamentTimesDisconnected: */ 0
            ))
            .FirstOrDefaultAsync();
        if (accountDetails is null)
        {
            return new NotFoundObjectResult(PHP.Serialize(new AuthFailedResponse(AuthFailureReason.AccountNotFound)));
        }

        await accountDetails.Load(bountyContext);
        
        Account account = accountDetails.Account;

        // If account exists, check the password first before checking for suspensions.
        string? proof = FinalizeSRPAuthentication(login, srpAuthSessionData, formData["proof"]);
        if (proof is null)
        {
            return new UnauthorizedObjectResult(PHP.Serialize(new AuthFailedResponse(AuthFailureReason.IncorrectPassword)));
        }

        SrpAuthResponse response = new(proof);
        var clientIpAddress = controllerContext.HttpContext.Connection.RemoteIpAddress.ToString();
        await AuthResponseConstruction.AssembleAuthResponseData(response, accountDetails, clientIpAddress, bountyContext);
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
        // calculation. 
        // The rfc5054 specification is unclear as to whether this 'should' be done or not. It
        // is done in the Python SRP library:
        // https://github.com/cocagne/pysrp/blob/4dfb111fffd671b7d97d803309fda2904e3ca1c8/srp/_pysrp.py#L205-L208
        // It's unclear if a similar change can (or should) merge this into the C# SRP library.
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
