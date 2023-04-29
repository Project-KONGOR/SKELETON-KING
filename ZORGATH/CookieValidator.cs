namespace ZORGATH;

public class CookieValidator : ICookieValidator
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CookieValidator(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<Account?> ValidateSessionCookie(string sessionCookie)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();
        using var bountyContext = serviceScope.ServiceProvider.GetService<BountyContext>()!;
        return await bountyContext.Accounts.Include(account => account.User).FirstOrDefaultAsync(account => account.Cookie == sessionCookie);
    }

    public async Task<Account?> ValidateSessionCookieForAccountId(string sessionCookie, string accountID)
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();
        using var bountyContext = serviceScope.ServiceProvider.GetService<BountyContext>()!;
        int id = int.Parse(accountID);
        return await bountyContext.Accounts.Include(account => account.User).FirstOrDefaultAsync(a => a.AccountId == id && a.Cookie == sessionCookie);
    }
}