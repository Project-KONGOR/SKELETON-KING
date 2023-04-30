namespace TRANSMUTANSTEIN;

[TestClass]
public class AutoCompleteNicksHandlerTest
{
    [TestMethod]
    public async Task TestNicknameHasExactMatch_HanderReturnsOK()
    {
        ControllerContext controllerContext = new ControllerContextForTesting();
        using BountyContext bountyContext = controllerContext.HttpContext.RequestServices.GetRequiredService<BountyContext>();
        bountyContext.Accounts.Add(
            new Account()
            {
                Name = "korDen"
            });
        await bountyContext.SaveChangesAsync();

        AutoCompleteNicksHandler handler = new AutoCompleteNicksHandler();

        Dictionary<string, string> formData = new()
        {
            ["nickname"] = "korDen"
        };

        var actual = await handler.HandleRequest(controllerContext, formData);
        var okObjectResult = actual as OkObjectResult;
        Assert.IsNotNull(okObjectResult);
        Assert.AreEqual("a:3:{s:5:\"nicks\";a:1:{i:0;s:6:\"korDen\";}s:16:\"vested_threshold\";i:5;i:0;b:1;}", okObjectResult.Value);
    }

    [TestMethod]
    public async Task TestNicknameHasMultipleMatches_HanderReturnsOK()
    {
        ControllerContext controllerContext = new ControllerContextForTesting();
        using BountyContext bountyContext = controllerContext.HttpContext.RequestServices.GetRequiredService<BountyContext>();
        bountyContext.Accounts.AddRange(
            new Account()
            {
                Name = "korNy"
            },
            new Account()
            {
                Name = "korDen"
            },
            new Account()
            {
                Name = "mrhappyasthma"
            });
        await bountyContext.SaveChangesAsync();

        AutoCompleteNicksHandler handler = new AutoCompleteNicksHandler();

        Dictionary<string, string> formData = new()
        {
            ["nickname"] = "ko"
        };

        var actual = await handler.HandleRequest(controllerContext, formData);
        var okObjectResult = actual as OkObjectResult;
        Assert.IsNotNull(okObjectResult);
        Assert.AreEqual("a:3:{s:5:\"nicks\";a:2:{i:0;s:6:\"korDen\";i:1;s:5:\"korNy\";}s:16:\"vested_threshold\";i:5;i:0;b:1;}", okObjectResult.Value);
    }

    [TestMethod]
    public async Task TestNicknameHasPartialMatch_HanderReturnsOK()
    {
        ControllerContext controllerContext = new ControllerContextForTesting();
        using BountyContext bountyContext = controllerContext.HttpContext.RequestServices.GetRequiredService<BountyContext>();
        bountyContext.Accounts.AddRange(
            new Account()
            {
                Name = "korDen"
            },
            new Account()
            {
                Name = "mrhappyasthma"
            });
        await bountyContext.SaveChangesAsync();

        AutoCompleteNicksHandler handler = new AutoCompleteNicksHandler();


        Dictionary<string, string> formData = new()
        {
            ["nickname"] = "kor"
        };

        var actual = await handler.HandleRequest(controllerContext, formData);
        var okObjectResult = actual as OkObjectResult;
        Assert.IsNotNull(okObjectResult);
        Assert.AreEqual("a:3:{s:5:\"nicks\";a:1:{i:0;s:6:\"korDen\";}s:16:\"vested_threshold\";i:5;i:0;b:1;}", okObjectResult.Value);
    }

    [TestMethod]
    public async Task TestNicknameHasNoMatch_HanderReturnsOK()
    {
        ControllerContext controllerContext = new ControllerContextForTesting();
        using BountyContext bountyContext = controllerContext.HttpContext.RequestServices.GetRequiredService<BountyContext>();
        bountyContext.Accounts.AddRange(
            new Account()
            {
                Name = "korDen"
            },
            new Account()
            {
                Name = "mrhappyasthma"
            });
        await bountyContext.SaveChangesAsync();

        AutoCompleteNicksHandler handler = new AutoCompleteNicksHandler();

        Dictionary<string, string> formData = new()
        {
            ["nickname"] = "asdf"
        };

        var actual = await handler.HandleRequest(controllerContext, formData);
        var okObjectResult = actual as OkObjectResult;
        Assert.IsNotNull(okObjectResult);
        Assert.AreEqual("a:2:{s:5:\"error\";s:5:\"FALSE\";i:0;b:0;}", okObjectResult.Value);
    }
}
