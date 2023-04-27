using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SecureRemotePassword;
using System.Collections.Concurrent;
using ZORGATH;

namespace TRANSMUTANSTEIN;

[TestClass]
public class PreAuthHandlerTest
{
    [TestMethod]
    public async Task PreAuthHandlerTest_AccountNotFound()
    {

        IServiceScopeFactory serviceScopeFactory = new ServiceScopeFactoryForTesting();
        ConcurrentDictionary<string, SrpAuthSessionData> srpAuthSessions = new();

        PreAuthHandler preAuthHandler = new(serviceScopeFactory, srpAuthSessions);
        ControllerContext controllerContext = new();

        Dictionary<string, string> formData = new()
        {
            ["login"] = "test",
            ["A"] = "A"
        };

        var result = await preAuthHandler.HandleRequest(controllerContext, formData);
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        Assert.AreEqual("a:2:{s:4:\"auth\";s:17:\"Account Not Found\";i:0;b:0;}", ((ObjectResult)result).Value);
    }

    [TestMethod]
    public async Task PreAuthHandlerTest_AccountIsFound()
    {
        ServiceScopeFactoryForTesting serviceScopeFactory = new();

        // Inject predefined SrpEphemeral to make the test deterministic.
        SrpEphemeral serverEphemeral = new()
        {
            Public = "public",
            Secret = "secret"
        };
        serviceScopeFactory.AddSingleton((SrpServer srpServer, string verifier) => serverEphemeral);

        using var bountyContext = serviceScopeFactory.CreateScope().ServiceProvider.GetService<BountyContext>()!;

        string login = "login";
        string a = "A";
        string salt = "0123456789ABCDEF";
        string passwordSalt = "passwordSalt";
        string hashedPassword = "hashedPassword";
        string verifier = "a9a86af68a05162f0a71623e396d9f0b0aa0bf08bd871ddd51964202db16d86202369cbe6a0417ab83e5954716ab30dd7ebb0dbfa2b121e05c7acdeb9822e72fa13fcb326e92a55c823adcf15234339a424f10a4c92bb13c00d039f236652d0b6e51e4d9b1488d1bb6de9b13f209e3ff43faa6bc8cd857442afcf83671961b52461b527fd296ecddef026dd9f773ab7d509fd85a32772b3490b88a0cb37a0c24b2b8ee11523c955be4cc549eaf5139330a1f3a54299d6e691fcd7893d22b0beec0f1379453eae053433d7efdfbeb8b83755340175dd7aff2f2fa453b3f456bbf6ab2bbeedff96c4b9416a76c8f040f69919660d00e9839eab333b630a1e9a1ec";
        bountyContext.Accounts.Add(
            new Account()
            {
                Name = login,
                User = new(salt, passwordSalt, hashedPassword)
            });
        await bountyContext.SaveChangesAsync();
        ConcurrentDictionary<string, SrpAuthSessionData> srpAuthSessions = new();

        PreAuthHandler preAuthHandler = new(serviceScopeFactory, srpAuthSessions);
        ControllerContext controllerContext = new();

        Dictionary<string, string> formData = new()
        {
            ["login"] = login,
            ["A"] = a,
        };

        var result = await preAuthHandler.HandleRequest(controllerContext, formData);
        // Should be a successful response.
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        // With correctly formatted response.
        Assert.AreEqual("a:5:{s:4:\"salt\";s:16:\"0123456789ABCDEF\";s:5:\"salt2\";s:12:\"passwordSalt\";s:1:\"B\";s:6:\"public\";s:16:\"vested_threshold\";i:5;i:0;b:1;}", ((ObjectResult)result).Value);

        // Should cache SrpAuthSessionData for srpAuth to pick up.
        Assert.AreEqual(
            srpAuthSessions[login], new SrpAuthSessionData(a, salt, passwordSalt, verifier, serverEphemeral)
        );
    }
}
