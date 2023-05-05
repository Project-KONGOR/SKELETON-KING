namespace ZORGATH;

[TestClass]
public class SrpAuthHandlerTest
{
    [TestMethod]
    public async Task TestSrpAuthMissingSession()
    {
        ConcurrentDictionary<string, SrpAuthSessionData> srpAuthSessions = new();
        SrpAuthHandler srpAuthHandler = new(srpAuthSessions, new());
        Dictionary<string, string> formData = new()
        {
            ["login"] = "dummyuser"
        };
        IActionResult result = await srpAuthHandler.HandleRequest(new ControllerContextForTesting(), formData);
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public async Task TestSrpAuthIncorrectProof()
    {
        string loginName = "dummyuser";

        // Precomputed Values: https://pastebin.com/c0TTziD6
        string clientPublicEphemeral = "4357180a10ca7393e09aadfbe43c807b3564a3b3916a3d48255e360b7ff7a7a1e5dd7616e2879d1c45d039a8f7bb84b29cd2195f79595608e654c89a2feceeb888ec117799b826d12ce8ec65937ba8b9a82998e3cdf049fda673a774e66977137540847ee359f09a4edcfcc9d2c7a8ea2b48e00b71db8e36e70e856c0dc0f2fc818710f54143a07fef459fa528a20d5fa55a4cce0ef8700a1926dd262f45aae0b270468dec3b4a212dc596ce9fcd1c149d392eebe98406ef5e250bcf509c819cad9af6e5dad4721ee845446c30d9f0b3c58e2f94cfe19c5d064d2e4cf2193efb71d1f752360655d3dd0aff327e73e12ef019e51b3cd14329b2f0aa7ebd96dd4b";
        string salt = "5dc38946324a5f866d4fcaf5f5c2777d52e168e8601f01bd7861d957455b6bee3f6e9617c0f5e958daff3ddc36f1f3ad075052ee13db0a9d77bbf6683786b9ffccdfbdf849370648f527442d66752ba07e1bfad5a5387c3be7f4df7d41425c78fbd2762502453fa491c9045bbe71fdce33d9d5afe1cc19d0d2515c389708cf3d2c9069b69e39de1580a567b651848ea7fdbcade3157b5c69caa53f886e36363e82fefce1a7f06ec333028d183c67da9aeef1cb6237774b85af230213e3159d34990cfdcabba62bfda64ccdcac45f278925044f834e38c5b271d48f652b6350bd5a82efac1e591ad2645c96c1652625d917a14753061670779afd69a5721a31ce";
        string passwordSalt = "b79bd4a832b46152f09ac9";
        string hashedPassword = "45b9467c354f712fa8b6175f6b6e82da5cb09a90fcfbda1e1d053dfc6aa41f5a";
        ConcurrentDictionary<string, SrpAuthSessionData> srpAuthSessions = new()
        {
            [loginName] = new(loginName: loginName, clientPublicEphemeral: clientPublicEphemeral, salt: salt, passwordSalt: passwordSalt, hashedPassword: hashedPassword, accountDetails: null!)
        };
        SrpAuthHandler srpAuthHandler = new(srpAuthSessions, new());
        Dictionary<string, string> formData = new()
        {
            ["login"] = loginName,
            ["proof"] = "invalid_proof"
        };
        IActionResult result = await srpAuthHandler.HandleRequest(new ControllerContextForTesting(), formData);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
    }

    [TestMethod]
    public async Task TestSrpAuthSuccess()
    {
        string loginName = "SouL";
        string clientPublicEphemeral = "2cf2668bf9d0e2358d43d41ea7ba3152e62f953519999ed8bf7ac94066c3974d2784e82d0e2841329d1188ebe8fa24ddab46a95e4882f801933ce2e4d3329d48fad794998429aa73786f2c6e0a8080672a7840ebfdb0135dc1276a04948a51a26785ddb64dc14e796c759c8a17368290ffdc6fe228c0fdf6b3a6e125cc4fbc532ee61442f6b2f8c44b7f288b60861d132aead63d8163c3bcf8f2651e47a600894a37997f2f9b9d273f1a82c6d4db545b78c882df45b79e4f05bb3bcaa4f0a58e6ed629a30f4b409627cef5a3943e8b342e2b9632099683b2f228c0b8c7086eaccd424b3c22df1fa599320ff21ee2510b4952a0a856bd0a6cf7cfd524fa5110e4";
        string salt = "078f6068db52fd1cea4db48789b3ce8bfb47fdb33d4767d0c86e35104a5d8ad511aa18624529bc3176697a4f366098aa016464cfcdf3b8acd620ecf5bd553e98ff4364b8f39ef6ffd2893bf80e681cd00d9622c1270ce2ddb681ab097c713429d5875566e2ef1934abb99d870ddf83f74be33b4b3ba22f7dd93b50057c45d5fe47a49539c61178d4c06748d5519433bd62e9f083bd59bbb4ed4f3a8b3fffb0895e07e07230445ec64fabc9a4e27c975012b9f78922beff6759150312d8b0fab25afbb363a2ed9810d6d1180636b0f886fc74dbba51f89389df06405308d7d9f0c2c2775985ae71c03337b0ecb938b327d8883e506632260962f8223d2f2865e2";
        string passwordSalt = "205cd38bd6ea0017322406";
        string verifier = "b0c856d02fc81d6e89d4ed0f6ab6f2b594349dc570c71b0182350568a55a78c082d515b30841ed28b4c72dd32d3cbb873ef7d9febf0c0f1f3a3870c90925b20ec88fcbdbbc89d5a1f8a7cf4c1c30c6c8428f031b1aa782bb8e0751f14998ba50364029a82344b48bd533c9c95a5e95211ea12cb0248ce3b21a974af4deb1a9cbe044d0b0df9aef81ad0a0424765ed4a2c22339c75dc0930e6768a2e59fb9bb7eee7da41e631c8a0cb8668efdd0f05b6050b90afa6dce4eab952c075f8d50809f9fe21dc230129a6ee9721e5a2adcf100bfe44c3cd672339cfbaf0f921523f901e3b5e5b53404c651d1f852b0f839520ca5f3f054e9b49651a8600031af54d7ee";
        SrpEphemeral serverEphemeral = new()
        {
            Public = "0f61bb9848afeeff12d24f3c1e37accb55fb702022df75c839028f06ec8b11fe0555f7ded43fbfd1d2062eca65093f35564c969a3494a7ed8d68c626e2e50b379f28f9a5cb65c2adac61080c2df78bec8d91ca416f2bcaf5a5d71e7f251fc28f31644ae61f746ba5d2449edf86f50d365a810921fab85ad7db9a815729b3d1731935d9accf07b3f7db8477b76ee7b4e7cc7cd08e4ca9ef60101cbb89c239676a67dc15fa849f6ec856ba04caf359d63ad17ea7444f3fc043a8fb26e8879c6b1b5fd51127c8e6c727c9badec4b2397dd40d38029b74262549d7282237aa95ad51c6dc1b2bef20d7b8726dbaa6c915a9747956f32c2a5c1bff1dbf4b3946636efb",
            Secret = "d7f620e7b625bc907374d2c92ed3d399827e2ba025a89227394135e74b38db3a"
        };
        AccountDetails accountDetails = new AccountDetails(
            accountId: 1,
            accountName: loginName,
            accountType: AccountType.Legacy,
            selectedUpgradeCodes: Array.Empty<string>(),
            autoConnectChatChannels: Array.Empty<string>(),
            ignoredAccountIds: Array.Empty<string>(),
            accountStats: new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
            clanId: null,
            clanName: null,
            clanTag: null,
            clanTier: Clan.Tier.None,
            useCloud: false,
            cloudAutoUpload: false,

            // User-specific information.
            userId: "fake-user-id",
            email: "fake-email-address",
            goldCoins: 12,
            silverCoins: 23,
            unlockedUpgradeCodes: Array.Empty<string>()
        );
        ConcurrentDictionary<string, SrpAuthSessionData> srpAuthSessions = new()
        {
            [loginName] = new(clientPublicEphemeral: clientPublicEphemeral, salt: salt, passwordSalt: passwordSalt, verifier: verifier, serverEphemeral: serverEphemeral, accountDetails: accountDetails)
        };
        SrpAuthHandler srpAuthHandler = new(srpAuthSessions, new());
        Dictionary<string, string> formData = new()
        {
            ["login"] = loginName,
            ["proof"] = "6523fe0eca24f04c280d9f6b9e5a2d3bec18686d773efdc35ed8f0b4f9057ca6"
        };

        ControllerContext controllerContext = new ControllerContextForTesting();
        BountyContext bountyContext = controllerContext.HttpContext.RequestServices.GetRequiredService<BountyContext>();

        bountyContext.Accounts.Add(new(name: loginName) { User = new() });
        await bountyContext.SaveChangesAsync();

        // Successful reponse.
        IActionResult result = await srpAuthHandler.HandleRequest(controllerContext, formData);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        // New cookie should be assigned to the Account.
        using BountyContext bountyContext2 = controllerContext.HttpContext.RequestServices.GetRequiredService<IDbContextFactory<BountyContext>>().CreateDbContext();
        string? cookie = await bountyContext2.Accounts.Where(a => a.Name == loginName).Select(a => a.Cookie).FirstOrDefaultAsync();
        Assert.IsNotNull(cookie);
    }
}
