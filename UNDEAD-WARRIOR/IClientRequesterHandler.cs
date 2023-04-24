﻿namespace UNDEAD_WARRIOR;

using Microsoft.AspNetCore.Mvc;

/// <summary>
///    Interface for a `client_requester.php` function request handlers.
/// </summary>
public interface IClientRequesterHandler
{
    /// <summary>
    ///    Handles a `client_requester.php` request with the given `formData`
    ///    and returns the appropriate result.
    /// </summary>
    public Task<IActionResult> HandleRequest(Dictionary<string, string> formData);
}
