using Microsoft.AspNetCore.Mvc;

namespace ZORGATH.GameAPI;

public class PlinkoController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}