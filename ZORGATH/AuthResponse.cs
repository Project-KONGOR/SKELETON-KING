using PhpSerializerNET;

namespace ZORGATH;

/// <summary>
///     The base HoN auth response.
/// </summary>
[Serializable]
public class AuthResponse
{
    /// <summary>
    ///     This is being used for the Tencent DDoS protection using Tencent watermarking verified by the proxy.
    /// </summary>
    [PhpProperty("sec_info")]
    public SecInfo SecInfo { get; } = new();

    /// <summary>
    ///     The ID of the logged-in account's master account.
    ///     This will be the same as the account ID after logging in with the master account.
    /// </summary>
    [PhpProperty("super_id")]
    public string? SuperId { get; set; }

    /// <summary>
    ///     The ID of the logged-in account, be it a master account or a sub-account.
    /// </summary>
    [PhpProperty("account_id")]
    public string? AccountId { get; set; }

    /// <summary>
    ///     The Garena ID of the logged-in account.
    ///     This property only applies to the Garena client.
    /// </summary>
    [PhpProperty("garena_id")]
    public string? GarenaId { get; set; }

    /// <summary>
    ///     The user name of the logged-in account.
    /// </summary>
    [PhpProperty("nickname")]
    public string? Nickname { get; set; }

    /// <summary>
    ///     The email address with which the logged-in account's user has registered with.
    /// </summary>
    [PhpProperty("email")]
    public string? Email { get; set; }

    /// <summary>
    ///     Unconfirmed - The user's account type. One of the `enum AccountType` values.
    /// </summary>
    [PhpProperty("account_type")]
    public string? AccountType { get; set; }

    /// <summary>
    ///     Unconfirmed. Potentially, a boolean as to whether the user's account is a trial account (i.e. back when users had some heroes locked).
    ///     Most probably set to "0" (false) in most cases.
    /// </summary>
    [PhpProperty("trial")]
    public string? Trial { get; set; }

    /// <summary>
    ///     Unconfirmed. Potentially, an integer pointing at the ID of a suspension issued by a GM.
    ///     A value of "0" would mean that the account is not currently suspended.
    /// </summary>
    [PhpProperty("susp_id")]
    public string? SuspensionId { get; set; }

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("prepay_only")]
    public string? PrepayOnly { get; set; }

    /// <summary>
    ///     Unconfirmed - Should be one of the `enum Standing` values.
    /// </summary>
    [PhpProperty("standing")]
    public string? Standing { get; set; }

    /// <summary>
    ///     Whether the user has any cloud download/upload settings.
    ///     This value is "1" when the option is set, or empty if unset.
    /// </summary>
    [PhpProperty("use_cloud")]
    public string? UseCloud { get; set; }

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("pass_exp")]
    public string? PassExp { get; set; }

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("is_new")]
    public string? IsNew { get; set; }

    /// <summary>
    ///     The authentication cookie for HoN that will be used to identify this user's
    ///     session in all subsequent requests..
    /// </summary>
    [PhpProperty("cookie")]
    public string? Cookie { get; set; }

    /// <summary>
    ///     The IP address of the client that connected to the server.
    /// </summary>
    [PhpProperty("ip")]
    public string? IPOfClient { get; set; }

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("minimum_ranked_level")]
    public string? MinimumRankedLevel { get; set; }

    /// <summary>
    ///     A decimal representation of the leaver percentage threshold. This is used by the client
    ///     to estimate how many more games player should play before they are allowed into a
    ///     "no leavers" match, TMM being one.
    /// </summary>
    [PhpProperty("leaverthreshold")]
    public double LeaverThreashold { get; set; } = 0.05;

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("is_under_24")]
    public bool? IsUnder24 { get; set; }

    /// <summary>
    ///     Whether or not this account has any subaccounts.
    /// </summary>
    [PhpProperty("is_subaccount")]
    public bool? IsSubaccount { get; set; }

    /// <summary>
    ///     Whether or not this login was for a subaccount. This identifies an account as
    ///     being a sub account.
    /// </summary>
    [PhpProperty("is_current_subaccount")]
    public bool? IsCurrentSubaccount { get; set; }

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("icb_url")]
    public string? IcbUrl { get; set; }

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("auth_hash")]
    public string? AuthHash { get; set; }

    /// <summary>
    ///     The UTC epoch time of the server. This value matches `timestamp` below.
    /// </summary>
    [PhpProperty("host_time")]
    public string? HostTime { get; set; }

    /// <summary>
    ///     The IP address of the chat server.
    /// </summary>
    [PhpProperty("chat_url")]
    public string? ChatUrl { get; set; }

    /// <summary>
    ///     The IP address of the chat server.
    /// </summary>
    [PhpProperty("chat_port")]
    public string? ChatPort { get; set; }

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("cafe_id")]
    public string? CafeId { get; set; }

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("gca_regular")]
    public string? GcaRegular { get; set; }

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("gca_prime")]
    public string? GcaPrime { get; set; }

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("commenting_url")]
    public string? CommentingUrl { get; set; }

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("commenting_port")]
    public int? CommentingPort { get; set; }

    /// <summary>
    ///     This field appears to be mostly obsolete. It returns the list of "free"
    ///     heroes in the rotation from when HoN first went free-to-play.
    ///     <br />
    ///     The latest value returned is "free" : "Hero_Genesis,Hero_Dorin_Tal,Hero_Adeve".
    /// </summary>
    [PhpProperty("hero_list")]
    public HeroList? HeroList { get; set; }

    /// <summary>
    ///     The user's buddy list. The dictionary should contain a single
    ///     entry with index of `account_id`. This dictionary contains a
    ///     list of BuddyListEntry entries. The index into that dictionary
    ///     is the friend's account ID.
    /// </summary>
    [PhpProperty("buddy_list")]
    public Dictionary<int, Dictionary<int, BuddyListEntry>>? BuddyList { get; set; }

    /// <summary>
    ///     The user's ignore list. The dictionary should contain a single
    ///     entry with index of `account_id`.
    /// </summary>
    [PhpProperty("ignored_list")]
    public Dictionary<int, List<IgnoredListEntry>>? IgnoredList { get; set; }

    /// <summary>
    ///     The user's ban list. The dictionary should contain a single
    ///     entry with index of `account_id`.
    /// </summary>
    [PhpProperty("banned_list")]
    public Dictionary<int, List<BannedListEntry>>? BannedList { get; set; }

    /// <summary>
    ///     This list only seems to contain a single entry. It's a bunch of
    ///     info related to the current user.
    /// </summary>
    [PhpProperty("infos")]
    public List<Info>? Info { get; set; }

    /// <summary>
    ///     Info for the user, if they are part of a clan.
    ///     <br />
    ///     If the user is not in a clan, set a ClanMemberInfoError instead.
    /// </summary>
    [PhpProperty("clan_member_info")]
    public ClanMemberInfo? ClanMemberInfo { get; set; }

    /// <summary>
    ///     A list of chat room names that the client is auto-connected to.
    ///     It seems like the first entry is an empty string sometimes and
    ///     not entirely sure why.
    /// </summary>
    [PhpProperty("chatrooms")]
    public List<string>? ChatRooms { get; set; }

    /// <summary>
    ///     The user's list of notifications.
    ///     <br />
    ///     NOTE: This does not include system mesesages. Those are handled separately.
    /// </summary>
    [PhpProperty("notifications")]
    public List<NotificationEntry>? Notifications { get; set; }

    /// <summary>
    ///     The list of all clan members.
    /// </summary>
    [PhpProperty("clan_roster")]
    public object? ClanRoster { get; set; }

    /// <summary>
    ///     The user's identities, listed in the order that they were registered.
    ///     The main account is listed first, followed by any sub-accounts.
    ///     <br />
    ///     The format is a 2-D array. Each element in the first array is a sub-array
    ///     of 2 elements: the username and the user ID.
    /// </summary>
    [PhpProperty("identities")]
    public List<List<string>>? Identities { get; set; }

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("points")]
    public string? Points { get; set; }

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("mmpoints")]
    public string? MmPoints { get; set; }

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("dice_tokens")]
    public string? DiceTokens { get; set; }

    /// <summary>
    ///     Unknown. I would have thought this was the rank of the user, but it doesn't appear to be.
    /// </summary>
    [PhpProperty("season_level")]
    public int? SeasonLevel { get; set; }

    // slot_id

    /// <summary>
    ///     A list of the user's unlocked upgrades. These are strings
    ///     to the upgrade ID. For example `aa.Hero_Pyromancer.Female`
    ///     is used for the Female Pyromancer alt avatar.
    ///     <br />
    ///     Prefixes seem to include:
    ///     h. (heroes -- e.g. 'AllHeroes')
    ///     m. (misc. 'Super Boost', 'Super-Taunt', 'allmodes.pass')
    ///     aa. (alt avatars)
    ///     t. (taunts)
    ///     ai. (icons)
    ///     cs. (flags)
    ///     c. (couriers)
    ///     eap (early access)
    ///     av (announcers)
    ///     te. (CoN rewards?)
    ///     ma. (mastery boost?)
    ///     en. (move upgrades??)
    /// </summary>
    [PhpProperty("my_upgrades")]
    public ICollection<string>? MyUpgrades { get; set; }

    /// <summary>
    ///     A list of the user's selected upgrades. These are strings
    ///     to the upgrade ID. For example `av.Samuel L Jackson` for
    ///     the Samuel L Jackson announcer.
    /// </summary>
    [PhpProperty("selected_upgrades")]
    public ICollection<string>? SelectedUpgrades { get; set; }

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("game_tokens")]
    public int? GameTokens { get; set; }

    /// <summary>
    ///     Metadata attached to each of the user's upgrades.
    ///     <br />
    ///     For consumable upgrades, such as mastery boosts, use type
    ///     MyUpgradesInfoEntryConsumable.
    /// </summary>
    [PhpProperty("my_upgrades_info")]
    public Dictionary<string, object>? MyUpgradesInfo { get; set; }

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("creep_level")]
    public int? CreepLevel { get; set; }

    /// <summary>
    ///     The UTC epoch time of the server. This value matches `host_time` above.
    /// </summary>
    [PhpProperty("timestamp")]
    public long? Timestamp { get; set; }

    /// <summary>
    ///     This seems to a bunch of miscellaneous static criteria regarding exp
    ///     and coin awards.
    /// </summary>
    [PhpProperty("awards_tooltip")]
    public AwardsTooltip? AwardsTooltip { get; set; }

    /// <summary>
    ///     This was used for the HoN quest system that has since been disabled.
    ///     It appears to be obsolete now.
    ///     <br />
    ///     On error, returns a single key of "error" and all QuestSystem values
    ///     set to 0.
    /// </summary>
    [PhpProperty("quest_system")]
    public Dictionary<string, QuestSystem>? QuestSystem { get; set; }

    /// <summary>
    ///     The current HoN campaign season. (The last season before servers went down was 12).
    /// </summary>
    [PhpProperty("campaign_current_season")]
    public string? CampaignCurrentSeason { get; set; }

    /// <summary>
    ///     Unknown - it appears to be a fixed string. Perhaps the mmr barriers between each rank?
    ///     "1250,1275,1300,1330,1365,1400,1435,1470,1505,1540,1575,1610,1645,1685,1725,1765,1805,1850,1900,1950"
    /// </summary>
    [PhpProperty("mmr_rank")]
    public string? MmrRank { get; set; }

    /// <summary>
    ///     An object containing the user's cloud storage settings. This
    ///     includes auto-download and auto-upload settings.
    /// </summary>
    [PhpProperty("account_cloud_storage_info")]
    public CloudStorageInfo? AccountCloudStorageInfo { get; set; }

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("mute_expiration")]
    public int? MuteExpiration { get; set; }

    /// <summary>
    ///     Unknown property which seems to often be set to "5", for some reason.
    /// </summary>
    [PhpProperty("vested_threshold")]
    public int VestedThreshold { get; } = 5;

    /// <summary>
    ///     Unknown property which seems to be set to true on a successful response or false if an error occurs.
    ///     If an error occurred, use "AuthFailedResponse" instead.
    /// </summary>
    [PhpProperty(0)]
    public bool Zero { get; } = true;
}

public class SecInfo
{
    /// <summary>
    ///     This is being used for the Tencent DDoS protection using Tencent watermarking verified by the proxy.
    ///     DO NOT CHANGE THESE VALUES !!!
    /// </summary>
    [PhpProperty("initial_vector")]
    public string InitialVector { get; } = "73088db5e71cfb6d";

    /// <summary>
    ///     It is unknown how this property is being used.
    /// </summary>
    [PhpProperty("hash_code")]
    public string HashCode { get; } = "73088db5e71cfb6d43ae0bb4abf095dd43862200";

    /// <summary>
    ///     It is unknown how this property is being used.
    /// </summary>
    [PhpProperty("key_version")]
    public string KeyVersion { get; } = "3e2d";
}

public class HeroList
{
    /// <summary>
    ///     I believe this to be the list of free heroes back from
    ///     when HoN was first free-to-play. It's hardcoded to the
    ///     current value sent from the HoN server, despite largely
    ///     being obsolete.
    /// </summary>
    [PhpProperty("free")]
    public string Free { get; } = "Hero_Genesis,Hero_Dorin_Tal,Hero_Adeve";
}

public class BuddyListEntry
{
    public BuddyListEntry(string nickname, int buddyId, int status, int standing, string clanTag, string group)
    {
        Nickname = nickname;
        BuddyId = buddyId;
        Status = status;
        Standing = standing; // 1: basic, 2: verified, 3: legacy
        ClanTag = clanTag;
        Group = group; // Todo
    }

    /// <summary>
    ///     The friend's account_id.
    /// </summary>
    [PhpProperty("buddy_id")]
    public int BuddyId { get; }

    /// <summary>
    ///     The friend group name, if any.
    /// </summary>
    [PhpProperty("group")]
    public string Group { get; }

    /// <summary>
    ///     The friend's clan tag, if any.
    /// </summary>
    [PhpProperty("clan_tag")]
    public string ClanTag { get; }

    /// <summary>
    ///     Unknown but seems to be set to "2" in all the cases I could find.
    /// </summary>
    [PhpProperty("status")]
    public int Status { get; }

    /// <summary>
    ///     The friend's username.
    /// </summary>
    [PhpProperty("nickname")]
    public string Nickname { get; }

    /// <summary>
    ///     Unknown but seems to be some kind of Enum value. I found values 1
    ///     through 3 in my friends list.
    /// </summary>
    [PhpProperty("standing")]
    public int Standing { get; }

    /// <summary>
    ///     Unknown. Seems to default to "0".
    /// </summary>
    [PhpProperty("inactive")]
    public string Inactive { get; } = "0";

    /// <summary>
    ///     Unknown. Seems to be "1" for true and "0" for false. But not
    ///     sure what qualifies as "new".
    /// </summary>
    [PhpProperty("new")]
    public string New { get; } = "0";
}

public class IgnoredListEntry
{
    /// <summary>
    ///     The ignored user's account_id.
    /// </summary>
    [PhpProperty("ignored_id")]
    public string? IgnoredId { get; set; }

    /// <summary>
    ///     The ignored user's username.
    /// </summary>
    [PhpProperty("nickname")]
    public string? Nickname { get; set; }

    public IgnoredListEntry(int ignoredId, string nickname)
    {
        IgnoredId = ignoredId.ToString();
        Nickname = nickname;
    }
}

public class BannedListEntry
{
    /// <summary>
    ///     The banned user's account_id.
    /// </summary>
    [PhpProperty("banned_id")]
    public string? BannedId { get; set; }

    /// <summary>
    ///     The ignored user's username.
    /// </summary>
    [PhpProperty("nickname")]
    public string? Nickname { get; set; }

    /// <summary>
    ///     The provided reason when using the '/banlist add' command.
    /// </summary>
    [PhpProperty("reason")]
    public string? Reason { get; set; }
}

public class Info
{
    /// <summary>
    ///     The ID of the logged-in account, be it a master account or a sub-account.
    /// </summary>
    [PhpProperty("account_id")]
    public string? AccountId { get; set; }

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("standing")]
    public string? Standing { get; set; }

    /// <summary>
    ///     The user's total account level.
    /// </summary>
    [PhpProperty("level")]
    public string? Level { get; set; }

    /// <summary>
    ///     The user's total account experience.
    /// </summary>
    [PhpProperty("level_exp")]
    public string? LevelExp { get; set; }

    /// <summary>
    ///     The total number of disconnects, including game
    ///     modes that are not tracked in the statistics page
    ///     (e.g. Devo Wars, and other custom maps).
    ///     <br />
    ///     NOTE: This may differ from the total disconnects
    ///     showed on the statistics page, which only counts
    ///     recorded game modes.
    /// </summary>
    [PhpProperty("discos")]
    public string? AllTimeTotalDisconnects { get; set; }

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("possible_discos")]
    public string? PossibleDisconnects { get; set; }

    /// <summary>
    ///     The total number of games played, including game
    ///     modes that are not tracked in the statistics page
    ///     (e.g. Devo Wars, and other custom maps).
    ///     <br />
    ///     NOTE: This may differ from the total disconnects
    ///     showed on the statistics page, which only counts
    ///     recorded game modes.
    /// </summary>
    [PhpProperty("games_played")]
    public string? AllTimeGamesPlayed { get; set; }

    /// <summary>
    ///     The number of bot games that the user has won.
    /// </summary>
    [PhpProperty("num_bot_games_won")]
    public string? NumBotGamesWon { get; set; }

    /// <summary>
    ///     The user's PSR (public skill rating) from stat recording
    ///     Public Games.
    /// </summary>
    [PhpProperty("acc_pub_skill")]
    public double PSR { get; set; }

    /// <summary>
    ///     The user's number of public game wins.
    /// </summary>
    [PhpProperty("acc_wins")]
    public int PublicGameWins { get; set; }

    /// <summary>
    ///     The user's number of public game losses.
    /// </summary>
    [PhpProperty("acc_losses")]
    public int PublicGameLosses { get; set; }

    /// <summary>
    ///     The number of public games played.
    /// </summary>
    [PhpProperty("acc_games_played")]
    public int PublicGamesPlayed { get; set; }

    /// <summary>
    ///     The number of disconnects from public games.
    /// </summary>
    [PhpProperty("acc_discos")]
    public int PublicGameDisconnects { get; set; }

    /// <summary>
    ///     The user's MMR (match making rating) from stat recording
    ///     Normal games (i.e. ranked games before CoN).
    /// </summary>
    [PhpProperty("rnk_amm_team_rating")]
    public string? NormalRankedGamesMMR { get; set; }

    /// <summary>
    ///     The user's number of normal ranked game wins.
    /// </summary>
    [PhpProperty("rnk_wins")]
    public string? NormalRankedGameWins { get; set; }

    /// <summary>
    ///     The user's number of normal ranked game losses.
    /// </summary>
    [PhpProperty("rnk_losses")]
    public string? NormalRankedGameLosses { get; set; }

    /// <summary>
    ///     The number of normal ranked games played.
    /// </summary>
    [PhpProperty("rnk_games_played")]
    public string? NormalRankedGamesPlayed { get; set; }

    /// <summary>
    ///     The number of disconnects from normal ranked games.
    /// </summary>
    [PhpProperty("rnk_discos")]
    public string? NormalRankedGameDisconnects { get; set; }

    /// <summary>
    ///     The user's MMR (match making rating) from stat recording
    ///     Normal games (i.e. ranked games before CoN).
    /// </summary>
    [PhpProperty("cs_amm_team_rating")]
    public string? CasualModeMMR { get; set; }

    /// <summary>
    ///     The user's number of casual mode wins.
    /// </summary>
    [PhpProperty("cs_wins")]
    public string? CasualModeWins { get; set; }

    /// <summary>
    ///     The user's number of casual mode losses.
    /// </summary>
    [PhpProperty("cs_losses")]
    public string? CasualModeLosses { get; set; }

    /// <summary>
    ///     The user's number of casual mode games played.
    /// </summary>
    [PhpProperty("cs_games_played")]
    public string? CasualModeGamesPlayed { get; set; }

    /// <summary>
    ///     The user's number of casual mode game disconnects.
    /// </summary>
    [PhpProperty("cs_discos")]
    public string? CasualModeDisconnects { get; set; }

    /// <summary>
    ///     The user's MMR (match making rating) from stat recording
    ///     MidWars games.
    /// </summary>
    [PhpProperty("mid_amm_team_rating")]
    public double MidWarsMMR { get; set; }

    /// <summary>
    ///     The user's number of midwars games played.
    /// </summary>
    [PhpProperty("mid_games_played")]
    public int MidWarsGamesPlayed { get; set; }

    /// <summary>
    ///     The user's number of midwars game disconnects.
    /// </summary>
    [PhpProperty("mid_discos")]
    public int MidWarsTimesDisconnected { get; set; }

    /// <summary>
    ///     The user's number of rift wars games played.
    /// </summary>
    [PhpProperty("rift_games_played")]
    public int RiftWarsGamesPlayed { get; set; }

    /// <summary>
    ///     The user's number of rift wars game disconnects.
    /// </summary>
    [PhpProperty("rift_discos")]
    public int RiftWarsDisconnects { get; set; }

    [PhpProperty("rift_amm_team_rating")]
    public double RiftWarsRating { get; set; }

    /// <summary>
    ///     Unknown. Seems to be 1 for true and 0 for false. But not
    ///     sure what qualifies as "new".
    /// </summary>
    [PhpProperty("is_new")]
    public int IsNew { get; set; }

    /// <summary>
    ///     The total number of CoN season ranked games across all seasons.
    /// </summary>
    [PhpProperty("cam_games_played")]
    public int ChampionsOfNewerthGamesPlayed { get; set; }

    /// <summary>
    ///     The total number of CoN season ranked game disconnects all seasons.
    /// </summary>
    [PhpProperty("cam_discos")]
    public int ChampionsOfNewerthGameDisconnects { get; set; }

    /// <summary>
    ///     The total number of CoN season casual ranked games across all seasons.
    /// </summary>
    [PhpProperty("cam_cs_games_played")]
    public int ChampionsOfNewerthCasualGamesPlayed { get; set; }

    /// <summary>
    ///     The total number of CoN season casual ranked game disconnects across all seasons.
    /// </summary>
    [PhpProperty("cam_cs_discos")]
    public int ChampionsOfNewerthCasualGameDisconnects { get; set; }

    // TODO: give proper names and annotate as PhpProperty.
    [PhpProperty("acc_herokills")]
    public int PublicHeroKills { get; set; }

    [PhpProperty("acc_heroassists")]
    public int PublicHeroAssists { get; set; }

    [PhpProperty("acc_deaths")]
    public int PublicDeaths { get; set; }

    [PhpProperty("acc_wards")]
    public int PublicWardsPlaced { get; set; }

    [PhpProperty("acc_gold")]
    public int PublicGoldEarned { get; set; }

    [PhpProperty("acc_exp")]
    public int PublicExpEarned { get; set; }

    [PhpProperty("acc_secs")]
    public int PublicSecondsPlayed { get; set; }

    [PhpProperty("acc_time_earning_exp")]
    public int PublicTimeEarningExp { get; set; }

    [PhpProperty("campaign_normal_mmr")]
    public double ChampionsOfNewerthNormalMMR { get; set; }

    [PhpProperty("campaign_normal_medal")]
    public int ChampionsOfNewerthNormalRank { get; set; }

    [PhpProperty("campaign_casual_mmr")]
    public double ChampionsOfNewerthCasualMMR { get; set; }

    [PhpProperty("campaign_casual_medal")]
    public int ChampionsOfNewerthCasualRank { get; set; }

    [PhpProperty("rnk_amm_solo_conf")]
    public int Rnk_amm_solo_conf { get; set; }

    [PhpProperty("rnk_amm_team_conf")]
    public int Rnk_amm_team_conf { get; set; }
}

public class ClanMemberInfo
{
    /// <summary>
    ///     The ID of the clan.
    /// </summary>
    [PhpProperty("clan_id")]
    public string? ClanId { get; set; }

    /// <summary>
    ///     The account ID of this user.
    /// </summary>
    [PhpProperty("account_id")]
    public string? AccountId { get; set; }

    /// <summary>
    ///     The rank of the user in the clan. Values are expected to be
    ///     strings from `enum ClanRank`.
    /// </summary>
    [PhpProperty("rank")]
    public string? Rank { get; set; }

    /// <summary>
    ///     Unknown?
    /// </summary>
    [PhpProperty("message")]
    public string? Message { get; set; }

    /// <summary>
    ///     The date the user joined the clan, in the following format:
    ///     "2020-06-23 09:33:02"
    /// </summary>
    [PhpProperty("join_date")]
    public string? JoinDate { get; set; }

    /// <summary>
    ///     The name of the clan.
    /// </summary>
    [PhpProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    ///     The clan's tag.
    /// </summary>
    [PhpProperty("tag")]
    public string? Tag { get; set; }

    /// <summary>
    ///     The account ID of the user that created the clan.
    /// </summary>
    [PhpProperty("creator")]
    public string? Creator { get; set; }

    /// <summary>
    ///     This field appears to be unused.
    /// </summary>
    [PhpProperty("title")]
    public string? Title { get; set; }

    /// <summary>
    ///     I'm not sure how they determine "active", but the value
    ///     is either "0" for false or "1" for true.
    /// </summary>
    [PhpProperty("active")]
    public string? Active { get; set; }

    /// <summary>
    ///     This field appears to be unused.
    /// </summary>
    [PhpProperty("logo")]
    public string? Logo { get; set; }

    /// <summary>
    ///     Unconfirmed - I'm guessing they used to give warnings to clans that
    ///     were idle (no active users) for too long. But it seems like it's
    ///     "0" for false and "1" for true.
    /// </summary>
    [PhpProperty("idleWarn")]
    public string? IdleWarning { get; set; }

    /// <summary>
    ///     This field appears to always be "0". I'm unsure what it's for.
    /// </summary>
    [PhpProperty("activeIndex")]
    public string? ActiveIndex { get; set; }
}

/// <summary>
///     Only use this class if the user is not in a clan.
/// </summary>
public class ClanMemberInfoError : ClanMemberInfo
{
    [PhpProperty("error")]
    public readonly string Error = "No Clan Member Found";
}

public class NotificationEntry
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
    public string FormattedNotification { get; set; }

    /// <summary>
    ///     The ID of the notification.
    /// </summary>
    [PhpProperty("notify_id")]
    public int NotifyId { get; set; }
}

public class ClanRosterEntry
{
    /// <summary>
    ///     The account ID of the clan member.
    /// </summary>
    [PhpProperty("account_id")]
    public int AccountId { get; set; }

    /// <summary>
    ///     The ID of the clan.
    /// </summary>
    [PhpProperty("clan_id")]
    public int ClanId { get; set; }

    /// <summary>
    ///     The rank of the user in the clan. Values are expected to be
    ///     strings from `enum ClanRank`.
    /// </summary>
    [PhpProperty("rank")]
    public string? Rank { get; set; }

    /// <summary>
    ///     Unknown?
    /// </summary>
    [PhpProperty("message")]
    public string? Message { get; set; }

    /// <summary>
    ///     The date the user joined the clan, in the following format:
    ///     "2020-06-23 09:33:02"
    /// </summary>
    [PhpProperty("join_date")]
    public string? JoinDate { get; set; }

    /// <summary>
    ///     The clan member's account name.
    /// </summary>
    [PhpProperty("nickname")]
    public string? Nickname { get; set; }

    /// <summary>
    ///     Unconfirmed - Should be one of the `enum Standing` values.
    /// </summary>
    [PhpProperty("standing")]
    public string? Standing { get; set; }
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

public class AwardsTooltip
{
    [PhpProperty("milestones")]
    public readonly MilestonesAwardTooltip Milestones = new();

    [PhpProperty("leveling")]
    public readonly LevelingAwardTooltip Leveling = new();

    [PhpProperty("bloodlust")]
    public readonly BloodlustAwardTooltip BloodlusAwardTooltipt = new();

    [PhpProperty("annihilation")]
    public readonly AnnihilationAwardTooltip Annihilation = new();

    [PhpProperty("immortal")]
    public readonly ImmortalAwardTooltip Immortal = new();

    [PhpProperty("victory")]
    public readonly VictoryAwardTooltip Victory = new();

    [PhpProperty("loss")]
    public readonly LossAwardTooltip Loss = new();

    [PhpProperty("disco")]
    public readonly DisconnectAwardTooltip Disconnect = new();

    [PhpProperty("quick")]
    public readonly QuickMatchAwardTooltip Quick = new();

    [PhpProperty("first")]
    public readonly FirstBloodAwardTooltip First = new();

    [PhpProperty("consec_win")]
    public readonly ConsecutiveWinAwardTooltip ConsecutiveWin = new();

    [PhpProperty("consec_loss")]
    public readonly ConsecutiveLossAwardTooltip ConsecutiveLoss = new();
}

public class MilestonesAwardTooltip
{
    [PhpProperty("heroassists")]
    public readonly MilestoneAwardTooltip HeroAssists = new(awardName: "heroassists", experience: 100, goblinCoins: 5, modulo: 250);

    [PhpProperty("herokills")]
    public readonly MilestoneAwardTooltip HeroKills = new(awardName: "herokills", experience: 100, goblinCoins: 5, modulo: 250);

    [PhpProperty("smackdown")]
    public readonly MilestoneAwardTooltip Smackdown = new(awardName: "smackdown", experience: 50, goblinCoins: 1, modulo: 10);

    [PhpProperty("wards")]
    public readonly MilestoneAwardTooltip Wards = new(awardName: "wards", experience: 100, goblinCoins: 5, modulo: 50);

    [PhpProperty("wins")]
    public readonly MilestoneAwardTooltip Wins = new(awardName: "wins", experience: 200, goblinCoins: 10, modulo: 50);
}

public class MilestoneAwardTooltip
{
    public MilestoneAwardTooltip(string awardName, int experience, int goblinCoins, int modulo)
    {
        AwardName = awardName;
        Experience = experience;
        GoblinCoins = goblinCoins;
        Modulo = modulo;
    }

    [PhpProperty("aname")]
    public readonly string AwardName;

    [PhpProperty("exp")]
    public readonly int Experience;

    [PhpProperty("gc")]
    public readonly int GoblinCoins;

    /// <summary>
    ///     The modulus used to determine the frequency in which the
    ///     milestone is hit. E.g. `10` would mean every 10 triggers.
    /// </summary>
    [PhpProperty("modulo")]
    public readonly int Modulo;
}

public class LevelingAwardTooltip
{
    [PhpProperty("2-5")]
    public readonly int TwoToFive = 6;

    [PhpProperty("6-10")]
    public readonly int SixToTen = 12;

    [PhpProperty("11-15")]
    public readonly int ElevenToFifteen = 16;
}

public class BloodlustAwardTooltip
{
    [PhpProperty("exp")]
    public readonly int Experience = 10;

    [PhpProperty("gc")]
    public readonly int GoblinCoins = 2;
}

public class AnnihilationAwardTooltip
{
    [PhpProperty("exp")] public readonly int Experience = 75;

    [PhpProperty("gc")]
    public readonly int GoblinCoins = 15; 

    [PhpProperty("tm_exp")]
    public readonly int TeamExperience = 25;

    [PhpProperty("tm_gc")]
    public readonly int TeamGoblinCoins = 5;
}

public class ImmortalAwardTooltip
{
    [PhpProperty("exp")]
    public readonly int Experience = 50;

    [PhpProperty("gc")]
    public readonly int GoblinCoins = 10;

    [PhpProperty("tm_exp")]
    public readonly int TeamExperience = 15;

    [PhpProperty("tm_gc")]
    public readonly int TeamGoblinCoins = 3;
}

public class VictoryAwardTooltip
{
    [PhpProperty("exp")]
    public readonly int Experience = 30;

    [PhpProperty("gc")]
    public readonly int GoblinCoins = 6;
}

public class LossAwardTooltip
{
    [PhpProperty("exp")]
    public readonly int Experience = 10;

    [PhpProperty("gc")]
    public readonly int GoblinCoins = 2;
}

public class DisconnectAwardTooltip
{
    [PhpProperty("exp")]
    public readonly int Experience = 0;

    [PhpProperty("gc")]
    public readonly int GoblinCoins = 0;
}

public class QuickMatchAwardTooltip
{
    [PhpProperty("exp")]
    public readonly int Experience = 0;

    [PhpProperty("gc")]
    public readonly int GoblinCoins = 2;
}

public class FirstBloodAwardTooltip
{
    [PhpProperty("exp")]
    public readonly int Experience = 20;

    [PhpProperty("gc")]
    public readonly int GoblinCoins = 4;
}

public class ConsecutiveWinAwardTooltip
{
    [PhpProperty("exp")]
    public readonly int Experience = 0;

    [PhpProperty("gc")]
    public readonly string GoblinCoins = "2-6";
}

public class ConsecutiveLossAwardTooltip
{
    [PhpProperty("exp")]
    public readonly int Experience = 0;

    [PhpProperty("gc")]
    public readonly int GoblinCoins = 1;
}

public class QuestSystem
{
    /// <summary>
    ///     Unknown. On error, returns 0.
    /// </summary>
    [PhpProperty("quest_status")]
    public readonly int QuestStatus = 0;

    /// <summary>
    ///     Unknown. On error, returns 0.
    /// </summary>
    [PhpProperty("leaderboard_status")]
    public readonly int LeaderboardStatus = 0;
}
