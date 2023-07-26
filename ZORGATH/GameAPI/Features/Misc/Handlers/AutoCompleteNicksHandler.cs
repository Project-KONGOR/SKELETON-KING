using ZORGATH.GameAPI.Features.Misc.Responses;
using ZORGATH.GameAPI.Support;

namespace ZORGATH.GameAPI.Features.Misc.Handlers;


public class AutoCompleteNicksHandler : IClientRequesterHandler
{
    public async Task<IActionResult> HandleRequest(ControllerContext controllerContext, Dictionary<string, string> formData)
    {
        string pattern = formData["nickname"];
        using BountyContext bountyContext = controllerContext.HttpContext.RequestServices.GetRequiredService<BountyContext>();
        List<string> matchingAccountNames = await bountyContext.Accounts
            .Where(account => account.Name.Contains(pattern))
            .Select(account => account.Name)
            .OrderBy(accountName => accountName)
            .Take(100)
            .ToListAsync();
        return new OkObjectResult(matchingAccountNames.Any() ?
            PHP.Serialize(new AutoCompleteNicksResponse(matchingAccountNames)) :
            PHP.Serialize(new AccountLookupErrorResponse()));
    }
}
