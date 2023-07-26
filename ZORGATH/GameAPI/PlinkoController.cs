using ZORGATH.GameAPI.Support;

namespace ZORGATH.GameAPI;

[ApiController]
[Route("__placeholder__master/casino/drop")]
[Consumes("application/x-www-form-urlencoded")]
public class PlinkoController : ControllerBase
{
    [HttpPost(Name = "Master Casino Drop Requester")]
    public IActionResult MasterCasinoDropRequester([FromForm] Dictionary<string, string> formData)
    {
        Dictionary<string, object> response = new();
        
        response["status_code"] = 1;
        Console.WriteLine("Plinko dropped");
        
        return Ok(PHP.Serialize(response));
    }
}
