namespace ZORGATH;

public record AccountInfo(string Salt, string PasswordSalt, string HashedPassword);

/// <summary>
/// client_requester.php?f=preAuth
/// Performs the first half of the SRP key exchange. Stores the intermediate results in a ConcurrentDictionary for the
/// second half of the SRP to validate the exchange.
/// </summary>
public class PreAuthHandler : IClientRequesterHandler
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ConcurrentDictionary<string, SrpAuthSessionData> _srpAuthSessions;

    public PreAuthHandler(IServiceScopeFactory serviceScopeFactory, ConcurrentDictionary<string, SrpAuthSessionData> srpAuthSessions)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _srpAuthSessions = srpAuthSessions;
    }

    public async Task<IActionResult> HandleRequest(ControllerContext controllerContext, Dictionary<string, string> formData)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();
        using var bountyContext = serviceScope.ServiceProvider.GetService<BountyContext>()!;
        string login = formData["login"];

        SrpAuthSessionData? srpAuthSessionData = await bountyContext.Accounts
            .Where(account => account.Name == login)
            .Select(account => new SrpAuthSessionData(
                login,
                formData["A"],
                account.User.Salt,
                account.User.PasswordSalt,
                account.User.HashedPassword))
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
