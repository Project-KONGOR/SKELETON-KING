using Microsoft.AspNetCore.Mvc;

namespace ZORGATH;

[ApiController]
[Route("client_requester.php")]
[Consumes("application/x-www-form-urlencoded")]

public class ClientRequesterController : ControllerBase
{
    private readonly Dictionary<string, IClientRequesterHandler> _clientRequesterHandlers;

    public ClientRequesterController(Dictionary<string,IClientRequesterHandler> clientRequesterHandlers)
    {
        _clientRequesterHandlers = clientRequesterHandlers;
    }

    [HttpPost(Name = "Client Requester")]
    public async Task<IActionResult> ClientRequester([FromForm] Dictionary<string, string> formData)
    {
        // Client requester has two forms for function identifiers. Some are
        // part of the query in the format `client_requester.php?f=`. Others
        // are specified as the `f` parameter in the `formData`.
        if (!formData.TryGetValue("f", out string? functionName))
        {
            functionName = Request.Query["f"].FirstOrDefault();
        }

        if (functionName == null)
        {
            // Unspecified request name.
            return BadRequest();
        }

        if (_clientRequesterHandlers.TryGetValue(functionName, out var requestHandler))
        {
            return await requestHandler.HandleRequest(ControllerContext, formData);
        }

        // Unknown request name.
        Console.WriteLine("Unknown request {0}", functionName);
        return BadRequest(functionName);
    }
}
