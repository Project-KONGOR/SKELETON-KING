namespace ZORGATH;

// A small subset of Account information that is sufficient to authenticate a player.
public class AccountDetails
{
    // Account-specific data.
    public readonly int AccountId;
    public readonly string AccountName;
    public readonly AccountType AccountType;
    public readonly ICollection<string> SelectedUpgradeCodes;
    public readonly ICollection<string> AutoConnectChatChannels;
    public readonly ICollection<string> IgnoredAccountIds;
    public readonly AccountStats AccountStats;
    public readonly string Cookie;
    public readonly int? ClanId;
    public readonly string? ClanName;
    public readonly string? ClanTag;
    public readonly Clan.Tier ClanTier;

    // User-specific data.
    public readonly string UserId;
    public readonly string Email;
    public readonly int GoldCoins;
    public readonly int SilverCoins;
    public readonly ICollection<string> UnlockedUpgradeCodes;
    public readonly bool UseCloud;
    public readonly bool CloudAutoUpload;

    // Loaded separately (and thus not readonly).
    public List<List<string>> Identities = null!;
    public Dictionary<int, BuddyListEntry> BuddyList = null!;
    public ICollection<NotificationEntry> Notifications = null!;
    public ICollection<IgnoredListEntry> IgnoredList = null!;
    public Dictionary<int, ClanRosterEntry>? ClanRoster = null;

    public AccountDetails(
        // Account-specific information.
        int accountId,
        string accountName,
        AccountType accountType,
        ICollection<string> selectedUpgradeCodes,
        ICollection<string> autoConnectChatChannels,
        ICollection<string> ignoredAccountIds,
        string cookie,
        AccountStats accountStats,

        // Clan information.
        int? clanId,
        string? clanName,
        string? clanTag,
        Clan.Tier clanTier,

        // CloudStorage.
        bool useCloud,
        bool cloudAutoUpload,

        // User-specific information.
        string userId,
        string email,
        int goldCoins,
        int silverCoins,
        ICollection<string> unlockedUpgradeCodes)
    {
        AccountId = accountId;
        AccountName = accountName;
        AccountType = accountType;
        SelectedUpgradeCodes = selectedUpgradeCodes;
        AutoConnectChatChannels = autoConnectChatChannels;
        IgnoredAccountIds = ignoredAccountIds;
        Cookie = cookie;
        AccountStats = accountStats;

        ClanId = clanId;
        ClanName = clanName;
        ClanTag = clanTag;
        ClanTier = clanTier;

        UserId = userId;
        Email = email;
        GoldCoins = goldCoins;
        SilverCoins = silverCoins;
        UnlockedUpgradeCodes = unlockedUpgradeCodes;

        UseCloud = useCloud;
        CloudAutoUpload = cloudAutoUpload;
    }

    public async Task Load(IDbContextFactory<BountyContext> bountyContextFactory)
    {
        var identities = bountyContextFactory.CreateDbContext().Accounts.Where(account => account.User.Id == UserId).Select(account => new List<string>() { account.Name, account.AccountId.ToString() }).ToListAsync();
        var buddyList = bountyContextFactory.CreateDbContext().Friends.Where(friend => friend.ExpirationDateTime == null && friend.AccountId == AccountId)
            .Select(friend => new BuddyListEntry(friend.FriendAccount.Name, friend.FriendAccount.AccountId, friend.FriendAccount.Clan!.Tag, friend.Group))
            .ToDictionaryAsync(
                buddyListEntry => buddyListEntry.BuddyId,
                buddyListEntry => buddyListEntry
            );
        var notifications = bountyContextFactory.CreateDbContext().Notifications.Where(n => n.AccountId == AccountId).Select(n => new NotificationEntry(n.Content, n.NotificationId)).ToListAsync();

        IEnumerable<int> ignoredAccountIds = IgnoredAccountIds.Select(int.Parse);
        var ignoredList = bountyContextFactory.CreateDbContext().Accounts.Where(a => ignoredAccountIds.Contains(a.AccountId)).Select(a => new IgnoredListEntry(a.AccountId, a.Name)).ToListAsync();

        Task<Dictionary<int, ClanRosterEntry>>? clanRoster;
        if (ClanId == null)
        {
            clanRoster = null;
        }
        else
        {
            clanRoster = bountyContextFactory.CreateDbContext().Accounts.Where(acc => acc.Clan!.ClanId == ClanId.Value)
                .Select(acc => new ClanRosterEntry(acc.AccountId, acc.ClanTier, acc.Name))
                .ToDictionaryAsync(entry => entry.AccountId, entry => entry);
        }

        var cookie = bountyContextFactory.CreateDbContext().Accounts
            .Where(account => account.AccountId == AccountId)
            .ExecuteUpdateAsync(update => update.SetProperty(account => account.Cookie, Cookie));

        // Wait until all 5 (or 6) requests are complete. This allows them to execute in parallel instead of sequentially.
        if (clanRoster == null)
        {
            await Task.WhenAll(identities, buddyList, notifications, ignoredList, cookie);
        }
        else
        {
            await Task.WhenAll(identities, buddyList, notifications, ignoredList, clanRoster, cookie);
            ClanRoster = clanRoster.Result;
        }

        Identities = identities.Result;
        BuddyList = buddyList.Result;
        Notifications = notifications.Result;
        IgnoredList = ignoredList.Result;
    }
}
