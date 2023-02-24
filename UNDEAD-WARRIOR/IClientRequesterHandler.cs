namespace UNDEAD_WARRIOR;

using Microsoft.AspNetCore.Mvc;

public interface IClientRequesterHandler
{
    public Task<IActionResult> HandleRequest(Dictionary<string, string> formData);
}
