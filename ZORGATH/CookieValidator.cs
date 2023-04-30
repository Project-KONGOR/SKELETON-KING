using System.Linq.Expressions;
using System;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore.Query;

namespace ZORGATH;

/// <summary>
///     Helper functions for cookie validation.
/// </summary>
public class CookieValidator
{
    /// <summary>
    ///     Validates the cookie and returns the corresponding Account for the session. In the event of an
    ///     invalid cookie, returns null.
    ///     
    ///     <param name="accounts">The Account entities from the database context. E.g. `bountyContext.accounts`.</param>
    ///     <param name="cookie"> The cookie string to authenticate.</param>
    ///     <param name="accountId">(Optional) An account ID to cross-reference with the account matching the `sessionCookie`.</param>
    ///     <param name="includes">
    ///         (Optional) A list of include lambdas to eagerly load alongside the the account. NOTE: This does not support nested includdes.
    ///         For instance, if you want to include the `User`, you could pass a list like so:
    ///
    ///                     List<Expression<Func<Account, object>>> includes = new()
    ///                     {
    ///                         (Account a) => a.User
    ///                     };
    ///     </param>
    /// </summary>
    public static async Task<Account?> ValidateSessionCookie(DbSet<Account> accounts, string cookie, string accountId = "", List<Expression<Func<Account, object>>>? includes = null)
    {
        // Apply each include statement for Eager Loading of related entities.
        IIncludableQueryable<Account, object>? query = null;
        if (includes != null && includes.Count > 0)
        {
            query = accounts.Include(includes[0]);
            for (int i = 1; i < includes.Count; i++)
            {
                query = query.Include(includes[i]);
            }
        }

        // Determine if we need to also match the AccountId as well as the cookie.
        Expression<Func<Account, bool>> condition = (Account a) => a.Cookie == cookie;
        if (accountId.Length > 0)
        {
            int id = int.Parse(accountId);
            condition = (Account a) => a.Cookie == cookie && a.AccountId == id;
        }

        if (query == null)
        {
            return await accounts.FirstOrDefaultAsync(condition);
        }
        return await query.FirstOrDefaultAsync(condition);
    }
}