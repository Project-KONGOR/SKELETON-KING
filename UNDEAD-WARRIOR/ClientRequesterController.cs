namespace UNDEAD_WARRIOR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

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

    [HttpGet(Name = "Client Requester")]
    public async Task<IActionResult> ClientRequester([FromForm] Dictionary<string, string> formData)
    {
        // Client requester has two forms for function identifiers. Some are
        // part of the query in the format `client_requester.php?f=`. Others
        // are specified as the `f` parameter in the `formData`.
        string? functionName;
        StringValues f = Request.Query["f"];

        if (f.Count == 1) { functionName = f[0]; }
        else { formData.TryGetValue("f", out functionName); }

        if (functionName == null)
        {
            // Unspecified request name.
            return BadRequest();
        }

        if (_clientRequesterHandlers.TryGetValue(functionName, out var requesterHandler))
        {
            return await requesterHandler.HandleRequest(formData);
        }

        // Unknown requet name.
        return BadRequest();
    }
}
