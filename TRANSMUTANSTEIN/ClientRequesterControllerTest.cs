namespace TRANSMUTANSTEIN;

[TestFixture]
public class ClientRequesterControllerTest
{
    private class TestClientRequesterHandler : IClientRequesterHandler
    {
        private readonly IActionResult _result;
        public TestClientRequesterHandler(IActionResult result)
        {
            _result = result;
        }

        public Task<IActionResult> HandleRequest(ControllerContext controllerContext, Dictionary<string, string> formData)
        {
            return Task.FromResult(_result);
        }
    }

    [Test]
    public async Task HandlerIsInvoked()
    {
        IActionResult expected = new OkResult();

        // Register our fake ClientRequesterHandler.
        Dictionary<string, IClientRequesterHandler> handlers = new()
        {
            ["test"] = new TestClientRequesterHandler(expected)
        };
        ClientRequesterController controller = new(handlers);

        Dictionary<string, string> formData = new()
        {
            ["f"] = "test"
        };

        // Trigger our fake ClientRequesterHandler.
        var actual = await controller.ClientRequester(formData);
        Assert.AreSame(expected, actual);
    }

    [Test]
    public async Task UnknownRequest()
    {
        ClientRequesterController controller = new(new Dictionary<string, IClientRequesterHandler>());
        Dictionary<string, string> formData = new()
        {
            ["f"] = "test2"
        };

        // Trigger an unknown request.
        var actual = await controller.ClientRequester(formData);
        Assert.IsInstanceOf<BadRequestObjectResult>(actual);
    }

    [Test]
    public async Task UnspecifiedRequest()
    {
        ClientRequesterController controller = new(new Dictionary<string, IClientRequesterHandler>());
        Dictionary<string, string> formData = new();

        // Trigger an unspecified request.
        var actual = await controller.ClientRequester(formData);
        Assert.IsInstanceOf<BadRequestObjectResult>(actual);
    }
}
