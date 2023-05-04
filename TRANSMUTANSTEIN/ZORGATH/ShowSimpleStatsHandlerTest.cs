namespace ZORGATH;

[TestClass]
public class ShowSimpleStatsHandlerTest
{
    [TestMethod]
    public async Task ShowSimpleStatsHandlerUnrecognizedCookie()
    {
        ShowSimpleStatsHandler handler = new();

        Dictionary<string, string> formData = new()
        {
            ["cookie"] = "what cookie?"
        };
        IActionResult result = await handler.HandleRequest(controllerContext: new ControllerContextForTesting(), formData: formData);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
    }

    [TestMethod]
    public async Task ShowSimpleStatsHandlerUnrecognizedPlayer()
    {
        ControllerContext controllerContext = new ControllerContextForTesting();
        using BountyContext bountyContext = controllerContext.HttpContext.RequestServices.GetRequiredService<BountyContext>();

        string login = "UnnamedNewbie";
        string cookie = "some cookie";
        // Precomputed Values: https://pastebin.com/c0TTziD6
        string salt = "5dc38946324a5f866d4fcaf5f5c2777d52e168e8601f01bd7861d957455b6bee3f6e9617c0f5e958daff3ddc36f1f3ad075052ee13db0a9d77bbf6683786b9ffccdfbdf849370648f527442d66752ba07e1bfad5a5387c3be7f4df7d41425c78fbd2762502453fa491c9045bbe71fdce33d9d5afe1cc19d0d2515c389708cf3d2c9069b69e39de1580a567b651848ea7fdbcade3157b5c69caa53f886e36363e82fefce1a7f06ec333028d183c67da9aeef1cb6237774b85af230213e3159d34990cfdcabba62bfda64ccdcac45f278925044f834e38c5b271d48f652b6350bd5a82efac1e591ad2645c96c1652625d917a14753061670779afd69a5721a31ce";
        string passwordSalt = "b79bd4a832b46152f09ac9";
        string hashedPassword = "45b9467c354f712fa8b6175f6b6e82da5cb09a90fcfbda1e1d053dfc6aa41f5a";
        bountyContext.Accounts.Add(new Account(login) { User = new(salt, passwordSalt, hashedPassword, email: "e@mail.com"), Cookie = cookie });
        await bountyContext.SaveChangesAsync();

        Dictionary<string, string> formData = new()
        {
            ["cookie"] = cookie,
            ["nickname"] = "what player?",
        };
        ShowSimpleStatsHandler handler = new();
        IActionResult result = await handler.HandleRequest(controllerContext, formData);
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    [TestMethod]
    public async Task ShowSimpleStatsHandlerSuccess()
    {
        ControllerContext controllerContext = new ControllerContextForTesting();
        using BountyContext bountyContext = controllerContext.HttpContext.RequestServices.GetRequiredService<BountyContext>();

        string login = "UnnamedNewbie";
        string cookie = "some cookie";
        // Precomputed Values: https://pastebin.com/c0TTziD6
        string salt = "5dc38946324a5f866d4fcaf5f5c2777d52e168e8601f01bd7861d957455b6bee3f6e9617c0f5e958daff3ddc36f1f3ad075052ee13db0a9d77bbf6683786b9ffccdfbdf849370648f527442d66752ba07e1bfad5a5387c3be7f4df7d41425c78fbd2762502453fa491c9045bbe71fdce33d9d5afe1cc19d0d2515c389708cf3d2c9069b69e39de1580a567b651848ea7fdbcade3157b5c69caa53f886e36363e82fefce1a7f06ec333028d183c67da9aeef1cb6237774b85af230213e3159d34990cfdcabba62bfda64ccdcac45f278925044f834e38c5b271d48f652b6350bd5a82efac1e591ad2645c96c1652625d917a14753061670779afd69a5721a31ce";
        string passwordSalt = "b79bd4a832b46152f09ac9";
        string hashedPassword = "45b9467c354f712fa8b6175f6b6e82da5cb09a90fcfbda1e1d053dfc6aa41f5a";
        bountyContext.Accounts.Add(new Account(login) { User = new(salt, passwordSalt, hashedPassword, email: "e@mail.com"), Cookie = cookie });
        await bountyContext.SaveChangesAsync();

        ShowSimpleStatsHandler handler = new();
        Dictionary<string, string> formData = new()
        {
            ["nickname"] = login,
            ["cookie"] = cookie,
        };
        IActionResult result = await handler.HandleRequest(controllerContext, formData);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        Assert.AreEqual("a:14:{s:8:\"nickname\";s:13:\"UnnamedNewbie\";s:5:\"level\";i:0;s:9:\"level_exp\";i:0;s:8:\"hero_num\";i:1239;s:10:\"avatar_num\";i:0;s:12:\"total_played\";i:0;s:7:\"mvp_num\";i:0;s:17:\"selected_upgrades\";a:0:{}s:10:\"account_id\";i:1;s:9:\"season_id\";i:22;s:13:\"season_normal\";a:4:{s:4:\"wins\";i:0;s:6:\"losses\";i:0;s:10:\"win_streak\";i:0;s:13:\"current_level\";i:0;}s:13:\"season_casual\";a:4:{s:4:\"wins\";i:0;s:6:\"losses\";i:0;s:10:\"win_streak\";i:0;s:13:\"current_level\";i:0;}s:15:\"award_top4_name\";a:0:{}s:14:\"award_top4_num\";a:0:{}}", ((OkObjectResult)result).Value);
    }
}
