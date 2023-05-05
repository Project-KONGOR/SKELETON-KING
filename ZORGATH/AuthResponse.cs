namespace ZORGATH;

/// <summary>
///     The base HoN auth response.
/// </summary>
[Serializable]
public class AuthResponse
{
    private static readonly string _authHashMagic = "8roespiemlasToUmiuglEhOaMiaSWlesplUcOAniupr2esPOeBRiudOEphiutOuJ";

    public AuthResponse(AccountDetails accountDetails, string cookie, string clientIpAddress, long hostTime, string chatServerUrl, string icbUrl, SecInfo secInfo)
    {
        AccountId = accountDetails.AccountId;
        AccountType = accountDetails.AccountType;
        AccountCloudStorageInfo = new CloudStorageInfo(accountDetails.UseCloud, accountDetails.CloudAutoUpload);
        AccountStats = new List<AccountStats>()
        {
            accountDetails.AccountStats
        };
        SelectedUpgrades = accountDetails.SelectedUpgradeCodes;
        MyUpgradesInfo = new();
        Nickname = accountDetails.AccountName;
        Email = accountDetails.Email;
        ClientIpAddress = clientIpAddress;
        Cookie = cookie;
        AuthHash = ComputeAuthHash(accountDetails.AccountId, cookie, clientIpAddress);
        HostTime = hostTime;
        VestedThreshold = 5;
        ChatUrl = chatServerUrl;
        if (accountDetails.ClanId != null)
        {
            ClanMemberInfo = new ClanMemberInfo(accountDetails.ClanName!, accountDetails.ClanId.Value, accountDetails.ClanTag!, accountDetails.ClanTier);
        }
        LeaverThreashold = 0.05f;
        IgnoredList = new()
        {
            [accountDetails.AccountId] = accountDetails.IgnoredList
        };
        ClanRoster = accountDetails.ClanRoster;
        Identities = accountDetails.Identities;
        BuddyList = new()
        {
            [accountDetails.AccountId] = accountDetails.BuddyList
        };
        ChatRooms = accountDetails.AutoConnectChatChannels;
        Notifications = accountDetails.Notifications;
        GoldCoins = accountDetails.GoldCoins;
        SilverCoins = accountDetails.SilverCoins;
        IcbUrl = icbUrl;
        SecInfo = secInfo;
    }

    /// <summary>
    ///   Helper function to compute the `authHash`.
    /// </summary>
    private static string ComputeAuthHash(int accountId, string cookie, string ip)
    {
        string authHashComputeString = accountId + ip + cookie + _authHashMagic;
        return Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(authHashComputeString))).ToLowerInvariant();
    }

    /// <summary>
    ///     The ID of the logged-in account, be it a master account or a sub-account.
    /// </summary>
    [PhpProperty("account_id")]
    public readonly int AccountId;

    /// <summary>
    ///     Unconfirmed - The user's account type. One of the `enum AccountType` values.
    /// </summary>
    [PhpProperty("account_type")]
    public readonly AccountType AccountType;

    /// <summary>
    ///     An object containing the user's cloud storage settings. This
    ///     includes auto-download and auto-upload settings.
    /// </summary>
    [PhpProperty("account_cloud_storage_info")]
    public readonly CloudStorageInfo AccountCloudStorageInfo;

    /// <summary>
    ///     This list only seems to contain a single entry. It's a bunch of
    ///     info related to the current user.
    /// </summary>
    [PhpProperty("infos")]
    public readonly ICollection<AccountStats> AccountStats;

    /// <summary>
    ///     A list of the user's selected upgrades. These are strings
    ///     to the upgrade ID. For example `av.Samuel L Jackson` for
    ///     the Samuel L Jackson announcer.
    /// </summary>
    [PhpProperty("selected_upgrades")]
    public readonly ICollection<string> SelectedUpgrades;

    /// <summary>
    ///     Metadata attached to each of the user's upgrades.
    ///     <br />
    ///     For consumable upgrades, such as mastery boosts, use type
    ///     MyUpgradesInfoEntryConsumable.
    /// </summary>
    [PhpProperty("my_upgrades_info")]
    public readonly Dictionary<string, UpgradeInfo> MyUpgradesInfo;

    /// <summary>
    ///     The user name of the logged-in account.
    /// </summary>
    [PhpProperty("nickname")]
    public readonly string Nickname;

    /// <summary>
    ///     The email address with which the logged-in account's user has registered with.
    /// </summary>
    [PhpProperty("email")]
    public readonly string Email;

    /// <summary>
    ///     The IP address of the client that connected to the server.
    /// </summary>
    [PhpProperty("ip")]
    public readonly string ClientIpAddress;

    /// <summary>
    ///     The authentication cookie for HoN that will be used to identify this user's
    ///     session in all subsequent requests..
    /// </summary>
    [PhpProperty("cookie")]
    public readonly string Cookie;

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("auth_hash")]
    public readonly string AuthHash;

    /// <summary>
    ///     The UTC epoch time of the server.
    /// </summary>
    [PhpProperty("host_time")]
    public readonly long HostTime;

    /// <summary>
    ///     Unknown property which seems to often be set to "5", for some reason.
    /// </summary>
    [PhpProperty("vested_threshold")]
    public readonly int VestedThreshold;

    /// <summary>
    ///     The IP address of the chat server.
    /// </summary>
    [PhpProperty("chat_url")]
    public readonly string ChatUrl;

    /// <summary>
    ///     Info for the user, if they are part of a clan.
    /// </summary>
    [PhpProperty("clan_member_info")]
    public readonly ClanMemberInfo? ClanMemberInfo;

    /// <summary>
    ///     A decimal representation of the leaver percentage threshold. This is used by the client
    ///     to estimate how many more games player should play before they are allowed into a
    ///     "no leavers" match, TMM being one.
    /// </summary>
    [PhpProperty("leaverthreshold")]
    public readonly float LeaverThreashold;

    /// <summary>
    ///     The user's ignore list. The dictionary should contain a single
    ///     entry with index of `account_id`.
    /// </summary>
    [PhpProperty("ignored_list")]
    public readonly Dictionary<int, ICollection<IgnoredListEntry>> IgnoredList;


    /// <summary>
    ///     The list of all clan members.
    /// </summary>
    [PhpProperty("clan_roster")]
    public readonly Dictionary<int, ClanRosterEntry>? ClanRoster;

    /// <summary>
    ///     The user's identities, listed in the order that they were registered.
    ///     The main account is listed first, followed by any sub-accounts.
    ///     <br />
    ///     The format is a 2-D array. Each element in the first array is a sub-array
    ///     of 2 elements: the username and the user ID.
    /// </summary>
    [PhpProperty("identities")]
    public readonly List<List<string>> Identities;

    /// <summary>
    ///     The user's buddy list. The dictionary should contain a single
    ///     entry with index of `account_id`. This dictionary contains a
    ///     list of BuddyListEntry entries. The index into that dictionary
    ///     is the friend's account ID.
    /// </summary>
    [PhpProperty("buddy_list")]
    public readonly Dictionary<int, Dictionary<int, BuddyListEntry>> BuddyList;

    /// <summary>
    ///     A list of chat room names that the client is auto-connected to.
    ///     It seems like the first entry is an empty string sometimes and
    ///     not entirely sure why.
    /// </summary>
    [PhpProperty("chatrooms")]
    public readonly ICollection<string> ChatRooms;

    /// <summary>
    ///     The user's list of notifications.
    ///     <br />
    ///     NOTE: This does not include system mesesages. Those are handled separately.
    /// </summary>
    [PhpProperty("notifications")]
    public readonly ICollection<NotificationEntry> Notifications;

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("points")]
    public readonly int GoldCoins;

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("mmpoints")]
    public readonly int SilverCoins;

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("icb_url")]
    public readonly string IcbUrl;

    /// <summary>
    ///     This is being used for the Tencent DDoS protection using Tencent watermarking verified by the proxy.
    /// </summary>
    [PhpProperty("sec_info")]
    public SecInfo SecInfo;
}

public readonly struct SecInfo
{
    public SecInfo(string initialVector = "", string hashCode = "", string keyVersion = "")
    {
        InitialVector = initialVector;
        HashCode = hashCode;
        KeyVersion = keyVersion;
    }

    /// <summary>
    ///     This is being used for the Tencent DDoS protection using Tencent watermarking verified by the proxy.
    /// </summary>
    [PhpProperty("initial_vector")]
    public readonly string InitialVector;

    /// <summary>
    ///     It is unknown how this property is being used.
    /// </summary>
    [PhpProperty("hash_code")]
    public readonly string HashCode;

    /// <summary>
    ///     It is unknown how this property is being used.
    /// </summary>
    [PhpProperty("key_version")]
    public readonly string KeyVersion;
}

public readonly struct UpgradeInfo
{
    public UpgradeInfo(string endTime, float discount, float mmpDiscount, int couponId, int productId, string couponProducts)
    {
        EndTime = endTime;
        Discount = discount;
        MmpDiscount = mmpDiscount;
        CouponId = couponId;
        ProductId = productId;
        CouponProducts = couponProducts;
    }

    [PhpProperty("end_time")]
    public readonly string EndTime;
    [PhpProperty("discount")]
    public readonly float Discount;
    [PhpProperty("mmp_discount")]
    public readonly float MmpDiscount;
    [PhpProperty("coupon_id")]
    public readonly int CouponId;
    [PhpProperty("product_id")]
    public readonly int ProductId;
    [PhpProperty("coupon_products")]
    public readonly string CouponProducts;
}

public readonly struct BuddyListEntry
{
    public BuddyListEntry(string nickname, int buddyId, string clanTag, string group)
    {
        Nickname = nickname;
        BuddyId = buddyId;
        ClanTag = clanTag;
        Group = group;
    }

    /// <summary>
    ///     The friend's account_id.
    /// </summary>
    [PhpProperty("buddy_id")]
    public readonly int BuddyId;

    /// <summary>
    ///     The friend group name, if any.
    /// </summary>
    [PhpProperty("group")]
    public readonly string Group;

    /// <summary>
    ///     The friend's clan tag, if any.
    /// </summary>
    [PhpProperty("clan_tag")]
    public readonly string ClanTag;

    /// <summary>
    ///     Unknown but seems to be set to "2" in all the cases I could find.
    /// </summary>
    [PhpProperty("status")]
    public readonly int Status = 2;

    /// <summary>
    ///     The friend's username.
    /// </summary>
    [PhpProperty("nickname")]
    public readonly string Nickname;

    /// <summary>
    ///     Unknown but seems to be some kind of Enum value. I found values 1
    ///     through 3 in my friends list.
    /// </summary>
    [PhpProperty("standing")]
    public readonly int Standing = 3; // 1: basic, 2: verified, 3: legacy

    /// <summary>
    ///     Unknown. Seems to default to "0".
    /// </summary>
    [PhpProperty("inactive")]
    public readonly string Inactive = "0";

    /// <summary>
    ///     Unknown. Seems to be "1" for true and "0" for false. But not
    ///     sure what qualifies as "new".
    /// </summary>
    [PhpProperty("new")]
    public readonly string New = "0";
}

public readonly struct IgnoredListEntry
{
    /// <summary>
    ///     The ignored user's account_id.
    /// </summary>
    [PhpProperty("ignored_id")]
    public readonly int IgnoredId;

    /// <summary>
    ///     The ignored user's username.
    /// </summary>
    [PhpProperty("nickname")]
    public readonly string Nickname;

    public IgnoredListEntry(int ignoredId, string nickname)
    {
        IgnoredId = ignoredId;
        Nickname = nickname;
    }
}

public readonly struct AccountStats
{
    public AccountStats(int level, float levelExp, int psr, float normalRankedGamesMMR, float casualModeMMR, int publicGamesPlayed, int normalRankedGamesPlayed, int casualModeGamesPlayed, int midWarsGamesPlayed, int allOtherGamesPlayed, int publicGameDisconnects, int normalRankedGameDisconnects, int casualModeDisconnects, int midWarsTimesDisconnected, int allOtherGameDisconnects)
    {
        Level = level;
        LevelExp = levelExp;
        PSR = psr;
        NormalRankedGamesMMR = normalRankedGamesMMR;
        CasualModeMMR = casualModeMMR;
        PublicGamesPlayed = publicGamesPlayed;
        NormalRankedGamesPlayed = normalRankedGamesPlayed;
        CasualModeGamesPlayed = casualModeGamesPlayed;
        MidWarsGamesPlayed = midWarsGamesPlayed;
        AllOtherGamesPlayed = allOtherGamesPlayed;
        PublicGameDisconnects = publicGameDisconnects;
        NormalRankedGameDisconnects = normalRankedGameDisconnects;
        CasualModeDisconnects = casualModeDisconnects;
        MidWarsTimesDisconnected = midWarsTimesDisconnected;
        AllOtherGameDisconnects = allOtherGameDisconnects;
    }

    /// <summary>
    ///     The user's total account level.
    /// </summary>
    [PhpProperty("level")]
    public readonly int Level;

    /// <summary>
    ///     The user's total account experience.
    /// </summary>
    [PhpProperty("level_exp")]
    public readonly float LevelExp;

    /// <summary>
    ///     The user's PSR (public skill rating) from stat recording
    ///     Public Games.
    /// </summary>
    [PhpProperty("acc_pub_skill")]
    public readonly int PSR;

    /// <summary>
    ///     The user's MMR (match making rating) from stat recording
    ///     Normal games (i.e. ranked games before CoN).
    /// </summary>
    [PhpProperty("rnk_amm_team_rating")]
    public readonly float NormalRankedGamesMMR;

    /// <summary>
    ///     The user's MMR (match making rating) from stat recording
    ///     Normal games (i.e. ranked games before CoN).
    /// </summary>
    [PhpProperty("cs_amm_team_rating")]
    public readonly float CasualModeMMR;

    /// <summary>
    ///     The user's number of public game wins.
    /// </summary>
    [PhpProperty("acc_games_played")]
    public readonly int PublicGamesPlayed;

    /// <summary>
    ///     The number of normal ranked games played.
    /// </summary>
    [PhpProperty("rnk_games_played")]
    public readonly int NormalRankedGamesPlayed;

    /// <summary>
    ///     The user's number of casual mode games played.
    /// </summary>
    [PhpProperty("cs_games_played")]
    public readonly int CasualModeGamesPlayed;

    /// <summary>
    ///     The user's number of midwars games played.
    /// </summary>
    [PhpProperty("mid_games_played")]
    public readonly int MidWarsGamesPlayed;

    /// <summary>
    ///     The user's number of all other games played.
    /// </summary>
    [PhpProperty("cam_games_played")]
    public readonly int AllOtherGamesPlayed;

    /// <summary>
    ///     The number of disconnects from public games.
    /// </summary>
    [PhpProperty("acc_discos")]
    public readonly int PublicGameDisconnects;

    /// <summary>
    ///     The number of disconnects from normal ranked games.
    /// </summary>
    [PhpProperty("rnk_discos")]
    public readonly int NormalRankedGameDisconnects;

    /// <summary>
    ///     The user's number of casual mode game disconnects.
    /// </summary>
    [PhpProperty("cs_discos")]
    public readonly int CasualModeDisconnects;

    /// <summary>
    ///     The user's number of midwars game disconnects.
    /// </summary>
    [PhpProperty("mid_discos")]
    public readonly int MidWarsTimesDisconnected;

    /// <summary>
    ///     The total number of CoN season ranked game disconnects all seasons.
    /// </summary>
    [PhpProperty("cam_discos")]
    public readonly int AllOtherGameDisconnects;
}

public readonly struct ClanMemberInfo
{
    public ClanMemberInfo(string clanName, int clanId, string clanTag, Clan.Tier clanTier)
    {
        Name = clanName;
        ClanId = clanId;
        Tag = clanTag;
        Rank = Clan.GetClanTierName(clanTier);
    }

    /// <summary>
    ///     The name of the clan.
    /// </summary>
    [PhpProperty("name")]
    public readonly string Name;

    /// <summary>
    ///     The ID of the clan.
    /// </summary>
    [PhpProperty("clan_id")]
    public readonly int ClanId;

    /// <summary>
    ///     The clan's tag.
    /// </summary>
    [PhpProperty("tag")]
    public readonly string Tag;

    /// <summary>
    ///     The rank of the user in the clan. Values are expected to be
    ///     strings from `enum ClanRank`.
    /// </summary>
    [PhpProperty("rank")]
    public readonly string Rank;
}

public readonly struct NotificationEntry
{
    public NotificationEntry(string formattedNotification, int notifyId)
    {
        FormattedNotification = formattedNotification;
        NotifyId = notifyId;
    }

    /// <summary>
    ///     A text string describing the notification in the format:
    ///     <br />
    ///     Format: "StevieSparkZ||2|notify_buddy_added|notification_generic_info||07/24 01:32 AM|1940041" "GameStop||23|notify_buddy_requested_added|notification_generic_action|action_friend_request|04/16 16:59 PM|2181677"
    ///     <br />
    ///     More inspection of this format is needed. The int value appears to be the enum value of the request. The final int is the notification ID.
    /// </summary>
    [PhpProperty("notification")]
    public readonly string FormattedNotification;

    /// <summary>
    ///     The ID of the notification.
    /// </summary>
    [PhpProperty("notify_id")]
    public readonly int NotifyId;
}

public readonly struct ClanRosterEntry
{
    public ClanRosterEntry(int accountId, Clan.Tier clanTier, string accountName)
    {
        AccountId = accountId;
        Rank = Clan.GetClanTierName(clanTier);
        Nickname = accountName;
    }

    /// <summary>
    ///     The account ID of the clan member.
    /// </summary>
    [PhpProperty("account_id")]
    public readonly int AccountId;

    /// <summary>
    ///     The rank of the user in the clan. Values are expected to be
    ///     strings from `enum ClanRank`.
    /// </summary>
    [PhpProperty("rank")]
    public readonly string Rank;

    /// <summary>
    ///     The clan member's account name.
    /// </summary>
    [PhpProperty("nickname")]
    public readonly string Nickname;
}

public class MyUpgradesInfoEntry
{
    /// <summary>
    ///     Unknown. Seems to always be an empty string.
    /// </summary>
    [PhpProperty("data")]
    public string? Data { get; set; }

    /// <summary>
    ///     Used for the end time (in UTC seconds) of a limited time upgrade, such as Early Access
    ///     heroes.
    /// </summary>
    [PhpProperty("end_time")]
    public string? EndTime { get; set; }

    /// <summary>
    ///     Used for the start time (in UTC seconds) of a limited time upgrade, such as Early Access
    ///     heroes.
    /// </summary>
    [PhpProperty("start_time")]
    public string? StartTime { get; set; }

    /// <summary>
    ///     Unknown. This value is unset if there is no score, or 0 if a score is set.
    /// </summary>
    [PhpProperty("used")]
    public int? Used { get; set; }

    /// <summary>
    ///     Unknown. This value is unset if there is no score. Otherwise it has some
    ///     integer like value.
    /// </summary>
    [PhpProperty("score")]
    public string? Score { get; set; }
}

public class MyUpgradesInfoEntryConsumable : MyUpgradesInfoEntry
{
    public MyUpgradesInfoEntryConsumable(int quantity, float discount, float mmpDiscount)
    {
        Quantity = quantity;
        Discount = discount;
        MmpDiscount = mmpDiscount;
    }

    /// <summary>
    ///     The quantity of remaining upgrades for a consumable upgrade type.
    ///     E.g. 5 superboosts remaining.
    /// </summary>
    [PhpProperty("quantity")]
    public readonly int Quantity;

    /// <summary>
    ///     Unknown. Seems to be "1.00" for mastery and super boosts.
    /// </summary>
    [PhpProperty("discount")]
    public readonly float Discount;

    /// <summary>
    ///     Unknown. Seems to be "1.00" for mastery and super boosts.
    /// </summary>
    [PhpProperty("mmp_discount")]
    public readonly float MmpDiscount;
}

public readonly struct CloudStorageInfo
{
    public CloudStorageInfo(bool useCloud, bool cloudAutoUpload)
    {
        UseCloud = useCloud;
        CloudAutoUpload = cloudAutoUpload;
    }

    /// <summary>
    ///     Whether the user has selected the option to automatically download
    ///     cloud settings on log-in.
    /// </summary>
    [PhpProperty("use_cloud")]
    public readonly bool UseCloud;

    /// <summary>
    ///     Whether the user has selected the option to automatically upload
    ///     any settings changes to the cloud.
    /// </summary>
    [PhpProperty("cloud_autoupload")]
    public readonly bool CloudAutoUpload;
}