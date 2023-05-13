namespace ZORGATH;

[ApiController]
[Route("patcher/patcher.php")]
[Consumes("application/x-www-form-urlencoded")]
public class PatcherController : ControllerBase
{
    [HttpPost(Name = "Patcher")]
    public IActionResult Patcher()
    {
        // Any OK response that doesn't contain a "version" field being set is treated as "up-to-date" response.
        // This will suppress an update prompt on the client since we always want updates to go through the launcher.
        // Note: we still want to tell the manager (but not individual game server instances) when there is a new
        // version available, but it's not handled here at the moment.
        return Ok(PHP.Serialize(new object()));
    }
}
