using System.Linq.Expressions;

namespace TRANSMUTANSTEIN;

[TestClass]
public class CookieValidatorTest
{
    private ControllerContext? ControllerContext { get; set; }

    [TestInitialize]
    public void SetUp()
    {
        ControllerContext = new ControllerContextForTesting();
        BountyContext bountyContext = ControllerContext.HttpContext.RequestServices.GetRequiredService<BountyContext>();
        Account account = EntityCreation.CreateFakeAccountAndSave("some username", bountyContext);
    }

    [TestCleanup]
    public void TearDown()
    {
        BountyContext bountyContext = ControllerContext!.HttpContext.RequestServices.GetRequiredService<BountyContext>();
        bountyContext.Dispose();
    }

    [TestMethod]
    public async Task ValidateSessionCookie()
    {
        BountyContext bountyContext = ControllerContext!.HttpContext.RequestServices.GetRequiredService<BountyContext>();

        Account? account = await CookieValidator.ValidateSessionCookie(bountyContext.Accounts, "cookie");

        Assert.IsNotNull(account);
        Assert.AreEqual(1, account!.AccountId);
    }

    [TestMethod]
    public async Task ValidateSessionCookie_FailsForInvalidCookie()
    {
        BountyContext bountyContext = ControllerContext!.HttpContext.RequestServices.GetRequiredService<BountyContext>();

        Account? account = await CookieValidator.ValidateSessionCookie(bountyContext.Accounts, "nonexistent cookie");

        Assert.IsNull(account);
    }

    [TestMethod]
    public async Task ValidateSessionCookieForAccount()
    {
        BountyContext bountyContext = ControllerContext!.HttpContext.RequestServices.GetRequiredService<BountyContext>();

        Account? account = await CookieValidator.ValidateSessionCookie(bountyContext.Accounts, "cookie", accountId: "1");

        Assert.IsNotNull(account);
    }

    [TestMethod]
    public async Task ValidateSessionCookieForAccount_FailsForInvalidCookie()
    {
        BountyContext bountyContext = ControllerContext!.HttpContext.RequestServices.GetRequiredService<BountyContext>();

        Account? account = await CookieValidator.ValidateSessionCookie(bountyContext.Accounts, "nonexistent cookie", accountId: "1");

        Assert.IsNull(account);
    }

    [TestMethod]
    public async Task ValidateSessionCookieForAccount_FailsForInvalidAccountId()
    {
        BountyContext bountyContext = ControllerContext!.HttpContext.RequestServices.GetRequiredService<BountyContext>();

        Account? account = await CookieValidator.ValidateSessionCookie(bountyContext.Accounts, "nonexistent cookie", accountId: "5"); // Actual ID is "1".

        Assert.IsNull(account);
    }

    [TestMethod]
    public async Task ValidateSessionCookieForAccount_CustomIncludes()
    {
        BountyContext bountyContext = ControllerContext!.HttpContext.RequestServices.GetRequiredService<BountyContext>();
        List<Expression<Func<Account, object>>> includes = new()
        {
            (Account a) => a.User
        };

        Account? account = await CookieValidator.ValidateSessionCookie(bountyContext.Accounts, "cookie", includes: includes);

        Assert.IsNotNull(account);

        // NOTE: The mock DB always loads navigational entites and I could not find a way to verify that the include actually works.
        // I did verify it manually, but if someone finds a way to do this in a unit test we should update this.
    }
}
