using Microsoft.EntityFrameworkCore;

namespace ZORGATH;

public readonly struct FriendInfo
{
    public readonly int AccountId;
    public readonly string NameWithClanTag;
    public readonly string ClanTag;
    public readonly string Group;
    public FriendInfo(int accountId, string nameWithClanTag, string clanTag, string group)
    {
        AccountId = accountId;
        NameWithClanTag = nameWithClanTag;
        ClanTag = clanTag;
        Group = group;
    }
}

public readonly struct NotificationInfo
{
    public readonly int NotificationId;
    public readonly string Content;

    public NotificationInfo(int notificationId, string content)
    {
        NotificationId = notificationId;
        Content = content;
    }
}

// A small subset of Account information that is sufficient to authenticate a player.
public class AccountDetailsForLogin : AccountDetails
{
    public readonly Account Account;
    public readonly string Email;
    public readonly string UserId;
    public readonly string UserName;
    public readonly int Points;
    public readonly int MMPoints;
    public readonly List<string> UnlockedUpgradeCodes;
    public readonly string? UseCloud;
    public readonly string? CloudAutoUpload;
    public readonly string? FileModifyTime;
    public readonly int? ClanId;
    public readonly string? ClanName;
    public readonly string? ClanTag;

    // Loaded separately (and thus not readonly) but never null.
    public List<List<string>> Identities = null!;
    public FriendInfo[] Friends = null!;
    public List<NotificationEntry> Notifications = null!;

    public AccountDetailsForLogin(
        string userId,
        string userName,
        Account account,
        string email,
        int points,
        int mmPoints,
        ICollection<string> unlockedUpgradeCodes,

        // Clan info
        int? clanId,
        string? clanName,
        string? clanTag,

        // CloudStorage
        string? useCloud,
        string? cloudAutoUpload,
        string? fileModifyTime,

        // base AccountDetails
        float publicRating,
        int publicGamesWon,
        int publicGamesLost,
        int publicTimesDisconnected,

        float midWarsRating,
        int midWarsGamesWon,
        int midWarsGamesLost,
        int midWarsTimesDisconnected,
        int midWarsPlacementsDone,

        float conNormalRating,
        int conNormalGamesWon,
        int conNormalGamesLost,
        int conNormalTimesDisconnected,
        int conNormalPlacementsDone,

        float conCasualRating,
        int conCasualGamesWon,
        int conCasualGamesLost,
        int conCasualTimesDisconnected,
        int conCasualPlacementsDone,

        int tournamentGamesWon,
        int tournamentGamesLost,
        int tournamentTimesDisconnected) : base(account.AccountId, publicRating, publicGamesWon, publicGamesLost, publicTimesDisconnected, midWarsRating, midWarsGamesWon, midWarsGamesLost, midWarsTimesDisconnected, midWarsPlacementsDone, conNormalRating, conNormalGamesWon, conNormalGamesLost, conNormalTimesDisconnected, conNormalPlacementsDone, conCasualRating, conCasualGamesWon, conCasualGamesLost, conCasualTimesDisconnected, conCasualPlacementsDone, tournamentGamesWon, tournamentGamesLost, tournamentTimesDisconnected)
    {
        Account = account;
        Email = email;
        UserId = userId;
        UserName = userName;
        Points = points;
        MMPoints = mmPoints;
        UnlockedUpgradeCodes = unlockedUpgradeCodes.ToList();
        ClanId = clanId;
        ClanName = clanName;
        ClanTag = clanTag;
        UseCloud = useCloud;
        CloudAutoUpload = cloudAutoUpload;
        FileModifyTime = fileModifyTime;
    }

    public async Task Load(BountyContext bountyContext)
    {
        Identities = await bountyContext.Accounts.Where(account => account.User.Id == UserId).Select(account => new List<string>() { account.Name, account.AccountId.ToString() }).ToListAsync();
        Friends = await bountyContext.Friends.Where(friend => friend.ExpirationDateTime == null && friend.AccountId == AccountId).Select(friend => new FriendInfo(friend.FriendAccount.AccountId, friend.FriendAccount.Name, friend.FriendAccount.Clan!.Tag, friend.Group)).ToArrayAsync();
        Notifications = await bountyContext.Notifications.Where(n => n.AccountId == AccountId).Select(n => new NotificationEntry(n.Content, n.NotificationId)).ToListAsync();
    }
}
