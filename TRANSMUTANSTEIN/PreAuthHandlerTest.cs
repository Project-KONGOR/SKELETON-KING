namespace TRANSMUTANSTEIN;

[TestClass]
public class PreAuthHandlerTest
{
    [TestMethod]
    public async Task PreAuthHandlerTest_AccountNotFound()
    {
        ConcurrentDictionary<string, SrpAuthSessionData> srpAuthSessions = new();

        PreAuthHandler preAuthHandler = new(srpAuthSessions);
        Dictionary<string, string> formData = new()
        {
            ["login"] = "test",
            // Precomputed 'A' value: https://pastebin.com/c0TTziD6
            ["A"] = "4357180a10ca7393e09aadfbe43c807b3564a3b3916a3d48255e360b7ff7a7a1e5dd7616e2879d1c45d039a8f7bb84b29cd2195f79595608e654c89a2feceeb888ec117799b826d12ce8ec65937ba8b9a82998e3cdf049fda673a774e66977137540847ee359f09a4edcfcc9d2c7a8ea2b48e00b71db8e36e70e856c0dc0f2fc818710f54143a07fef459fa528a20d5fa55a4cce0ef8700a1926dd262f45aae0b270468dec3b4a212dc596ce9fcd1c149d392eebe98406ef5e250bcf509c819cad9af6e5dad4721ee845446c30d9f0b3c58e2f94cfe19c5d064d2e4cf2193efb71d1f752360655d3dd0aff327e73e12ef019e51b3cd14329b2f0aa7ebd96dd4b"
        };

        IActionResult result = await preAuthHandler.HandleRequest(new ControllerContextForTesting(), formData);
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        Assert.AreEqual("a:2:{s:4:\"auth\";s:17:\"Account Not Found\";i:0;b:0;}", ((ObjectResult)result).Value);
    }

    [TestMethod]
    public async Task PreAuthHandlerTest_AccountIsFound()
    {
        ControllerContext controllerContext = new ControllerContextForTesting();
        using BountyContext bountyContext = controllerContext.HttpContext.RequestServices.GetRequiredService<BountyContext>();

        string login = "login";
        // Precomputed Values: https://pastebin.com/c0TTziD6
        string clientPublicEphemeral = "4357180a10ca7393e09aadfbe43c807b3564a3b3916a3d48255e360b7ff7a7a1e5dd7616e2879d1c45d039a8f7bb84b29cd2195f79595608e654c89a2feceeb888ec117799b826d12ce8ec65937ba8b9a82998e3cdf049fda673a774e66977137540847ee359f09a4edcfcc9d2c7a8ea2b48e00b71db8e36e70e856c0dc0f2fc818710f54143a07fef459fa528a20d5fa55a4cce0ef8700a1926dd262f45aae0b270468dec3b4a212dc596ce9fcd1c149d392eebe98406ef5e250bcf509c819cad9af6e5dad4721ee845446c30d9f0b3c58e2f94cfe19c5d064d2e4cf2193efb71d1f752360655d3dd0aff327e73e12ef019e51b3cd14329b2f0aa7ebd96dd4b";
        string salt = "5dc38946324a5f866d4fcaf5f5c2777d52e168e8601f01bd7861d957455b6bee3f6e9617c0f5e958daff3ddc36f1f3ad075052ee13db0a9d77bbf6683786b9ffccdfbdf849370648f527442d66752ba07e1bfad5a5387c3be7f4df7d41425c78fbd2762502453fa491c9045bbe71fdce33d9d5afe1cc19d0d2515c389708cf3d2c9069b69e39de1580a567b651848ea7fdbcade3157b5c69caa53f886e36363e82fefce1a7f06ec333028d183c67da9aeef1cb6237774b85af230213e3159d34990cfdcabba62bfda64ccdcac45f278925044f834e38c5b271d48f652b6350bd5a82efac1e591ad2645c96c1652625d917a14753061670779afd69a5721a31ce";
        string passwordSalt = "b79bd4a832b46152f09ac9";
        string hashedPassword = "45b9467c354f712fa8b6175f6b6e82da5cb09a90fcfbda1e1d053dfc6aa41f5a";
        string verifier = "146de93c988381b36ae601f2b19834cca2a4c0834d8b1a93545d8ec4539e1eb20fb70659ea85c78bf2e9692ff0e8c0fb8e5a8d0d49c259172593a5955ce5f6a519486a87e11763da7853a77828ef5791fe002db90e4a82e67a6cfc701c2a41db3377cb732660cea572046bca8d03d29bfc049e0110996cdaf4dcd09882a20424f6cf92ea571693c953dba67417dfb560498d159d4d21805027d43b5b4688ab99c51d9c02c8c07c0d0ac4cef2bb753d63fc6d3cfe49755b49e698208a72468a7faae6fbda1542af014a2fa6b3c5b310acc0df49aa8f7034f3f8c0d47accedab805c0384cd35589bcabc0ffbf800b20bc08aee87f102bf47e9cc0aaeb91a22e272";
        bountyContext.Accounts.Add(new Account(login) { User = new(salt, passwordSalt, hashedPassword, email: "e@mail.com") });
        await bountyContext.SaveChangesAsync();
        ConcurrentDictionary<string, SrpAuthSessionData> srpAuthSessions = new();

        PreAuthHandler preAuthHandler = new(srpAuthSessions);
        Dictionary<string, string> formData = new()
        {
            ["login"] = login,
            ["A"] = clientPublicEphemeral,
        };

        IActionResult result = await preAuthHandler.HandleRequest(controllerContext, formData);

        // Should be a successful response.
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        // Should cache SrpAuthSessionData for srpAuth to pick up.
        SrpAuthSessionData actualSrpAuthSessionData = srpAuthSessions[login];
        Assert.AreEqual(clientPublicEphemeral, actualSrpAuthSessionData.ClientPublicEphemeral);
        Assert.AreEqual(salt, actualSrpAuthSessionData.Salt);
        Assert.AreEqual(passwordSalt, actualSrpAuthSessionData.PasswordSalt);
        Assert.AreEqual(verifier, actualSrpAuthSessionData.Verifier);

        // With correctly formatted response.
        string expectedResponse = "a:5:{s:4:\"salt\";s:512:\"5dc38946324a5f866d4fcaf5f5c2777d52e168e8601f01bd7861d957455b6bee3f6e9617c0f5e958daff3ddc36f1f3ad075052ee13db0a9d77bbf6683786b9ffccdfbdf849370648f527442d66752ba07e1bfad5a5387c3be7f4df7d41425c78fbd2762502453fa491c9045bbe71fdce33d9d5afe1cc19d0d2515c389708cf3d2c9069b69e39de1580a567b651848ea7fdbcade3157b5c69caa53f886e36363e82fefce1a7f06ec333028d183c67da9aeef1cb6237774b85af230213e3159d34990cfdcabba62bfda64ccdcac45f278925044f834e38c5b271d48f652b6350bd5a82efac1e591ad2645c96c1652625d917a14753061670779afd69a5721a31ce\";s:5:\"salt2\";s:22:\"b79bd4a832b46152f09ac9\";s:1:\"B\";s:512:\"" + actualSrpAuthSessionData.ServerEphemeral.Public + "\";s:16:\"vested_threshold\";i:5;i:0;b:1;}";
        string actualResponse = (string)((ObjectResult)result).Value;
        Assert.AreEqual(expectedResponse, actualResponse);
    }
}
