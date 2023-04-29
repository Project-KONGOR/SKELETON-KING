namespace TRANSMUTANSTEIN;

[TestClass]
public class CookieValidatorTest
{
    private ControllerContext? ControllerContext { get; set; }

    [TestInitialize]
    public void SetUp()
    {
        ControllerContext = new ControllerContextForTesting();
        using BountyContext bountyContext = ControllerContext.HttpContext.RequestServices.GetRequiredService<BountyContext>();
        Account account = EntityCreation.CreateFakeAccountAndSave("some username", bountyContext);
        account.Cookie = "cookie";
        bountyContext.SaveChanges();
    }

    [TestMethod]
    public async Task ValidateSessionCookie()
    {
        ICookieValidator validator = ControllerContext!.HttpContext.RequestServices.GetRequiredService<ICookieValidator>();
        Account? account = await validator.ValidateSessionCookie("cookie");

        Assert.IsNotNull(account);
        Assert.AreEqual(1, account!.AccountId);
    }

    [TestMethod]
    public async Task ValidateSessionCookie_FailsForInvalidCookie()
    {
        ICookieValidator validator = ControllerContext!.HttpContext.RequestServices.GetRequiredService<ICookieValidator>();

        Account? account = await validator.ValidateSessionCookie("nonexistent cookie");

        Assert.IsNull(account);
    }

    [TestMethod]
    public async Task ValidateSessionCookieForAccount()
    {
        ICookieValidator validator = ControllerContext!.HttpContext.RequestServices.GetRequiredService<ICookieValidator>();

        Account? account = await validator.ValidateSessionCookieForAccountId("cookie", "1");

        Assert.IsNotNull(account);
    }

    [TestMethod]
    public async Task ValidateSessionCookieForAccount_FailsForInvalidCookie()
    {
        ICookieValidator validator = ControllerContext!.HttpContext.RequestServices.GetRequiredService<ICookieValidator>();

        Account? account = await validator.ValidateSessionCookieForAccountId("nonexistent cookie", "1");

        Assert.IsNull(account);
    }

    [TestMethod]
    public async Task ValidateSessionCookieForAccount_FailsForInvalidAccountId()
    {
        ICookieValidator validator = ControllerContext!.HttpContext.RequestServices.GetRequiredService<ICookieValidator>();

        Account? account = await validator.ValidateSessionCookieForAccountId("cookie", "5");  // Actual ID is "1".

        Assert.IsNull(account);
    }
}
