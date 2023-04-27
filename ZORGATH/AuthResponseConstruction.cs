using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ZORGATH;

public class AuthResponseConstruction
{
    private static readonly string _authHashMagic = "8roespiemlasToUmiuglEhOaMiaSWlesplUcOAniupr2esPOeBRiudOEphiutOuJ";
    /// <summary>
    ///     Assembles the given `response` by setting all of the data fields on an auth response.
    ///     
    ///     The HoN auth responses contain a ton of data, so this helper function assembles all
    ///     of that data from various sources and sets them properly on the response.
    /// </summary>
    /// 
    /// <param name="response">
    ///     The response object to set all of the user data fields on.
    /// </param>
    /// <param name="session">
    ///     The current user's Session data.
    /// </param>
    /// <param name="chatServerConfig">
    ///     The chat server configuration.
    /// </param>
    /// 
    /// <returns>
    ///     Returns the assembled SRP Auth response that includes all of the data from our various tables.
    /// </returns>
    public static async Task AssembleAuthResponseData(AuthResponse response, AccountDetailsForLogin accountDetails, string clientIpAddress, BountyContext bountyContext)
    {
        long currentTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        string icbUrl = "http://api.kongor.online"; // TODO: fixme

        // ******
        // NOTE: Fields are listed in the order they appear in the actual Auth response payload,
        // purely as a convenience / convention. Any unimplmented fields are annotated as such
        // in a comment.
        //
        // If more than a line or two is needed for a field (e.g. iterating all friends for the
        // friends list), please make a helper function to keep this method maintainable.
        // ******

        Account account = accountDetails.Account;
        // Unimplemented field: response.SecInfo
        // Unimplemented field: response.SuperId
        response.AccountId = account.AccountId.ToString();
        // Unimplemented field: response.GarenaId
        response.Nickname = account.Name;
        response.Email = accountDetails.Email;
        response.AccountType = ((int)account.AccountType).ToString();
        // Unimplemented field: response.Trial
        // Unimplemented field: response.SuspensionId
        // Unimplemented field: response.PrepayOnly
        response.Standing = "3";
        response.UseCloud = accountDetails.UseCloud ?? "0";
        // Unimplemented field: response.PassExp
        response.IsNew = "0";
        response.Cookie = account.Cookie;
        response.IPOfClient = clientIpAddress;
        // Unimplemented field: response.MinimumRankedLevel
        // Unimplemented field: response.IsUnder24
        // Unimplemented field: response.IsSubaccount
        // Unimplemented field: response.IsCurrentSubaccount
        response.IcbUrl = icbUrl;
        response.AuthHash = ComputeAuthHash(account, clientIpAddress);
        response.HostTime = currentTime.ToString();
        response.ChatUrl = "127.0.0.1"; // TODO: fixme
        response.ChatPort = "12345";    // TODO: fixme
        // Unimplemented field: response.CafeId
        // Unimplemented field: response.GcaRegular
        // Unimplemented field: response.GcaPrime
        // Unimplemented field: response.CommentingUrl
        // Unimplemented field: response.CommentingPort
        // Unimplemented field: response.HeroList
        response.BuddyList = BuddyListForAccount(account, accountDetails.Friends);
        response.IgnoredList = await IgnoredListForAccount(account, bountyContext);
        // TODO: Implement the banned list. (A placeholder is included to avoid a crash in the game client.)
        response.BannedList = BannedListForAccount(account);
        // TODO: Properly implement `Info`.  Game client appears to be crashing
        // if not set, so just use a placeholder for now.
        response.Info = InfoForAccount(accountDetails, tournamentRatingForActiveTeam: 0); // Note: tournament rating is irrelevant here.
        response.ClanMemberInfo = ClanMemberInfoForAccount(accountDetails);
        response.ChatRooms = account.AutoConnectChatChannels.ToList();
        // KONGOR is a special channel name that maps to "KONGOR 1", then "KONGOR 2" if "KONGOR 1" is full, then "KONGOR 3" etc.
        response.ChatRooms.Add("KONGOR");
        if (accountDetails.ClanName != null) response.ChatRooms.Add($"Clan {accountDetails.ClanName}");
        // Unimplemented field: response.HeroList
        response.Notifications = accountDetails.Notifications;
        int? clanId = accountDetails.ClanId;
        if (clanId is null)
        {
            response.ClanRoster = new Dictionary<string, string> { { "error", "No Clan Members Found" } };
        }
        else
        {
            response.ClanRoster = await ClanRosterForClan(clanId.Value, bountyContext);
        }
        
        response.Identities = accountDetails.Identities;
        response.Points = accountDetails.Points.ToString();
        response.MmPoints = accountDetails.MMPoints.ToString();
        // Unimplemented field: response.DiceTokens
        // Unimplemented field: response.SeasonLevel
        response.MyUpgrades = accountDetails.UnlockedUpgradeCodes;
        response.SelectedUpgrades = account.SelectedUpgradeCodes;
        // Unimplemented field: response.GameTokens
        response.MyUpgradesInfo = MyUpgradesInfoForAccount(accountDetails);
        // Unimplemented field: response.CreepLevel
        response.Timestamp = currentTime;
        // Unimplemented field: response.AwardsTooltip
        // Unimplemented field: response.QuestSystem
        // Unimplemented field: response.CampaignCurrentSeason
        response.MmrRank = ChampionsOfNewerthRanks.MmrByRankLookupTableString;
        response.AccountCloudStorageInfo = CloudStorageInfoForAccount(account.AccountId, accountDetails.UseCloud, accountDetails.CloudAutoUpload, accountDetails.FileModifyTime);
        // Unimplemented field: response.MuteExpiration
    }

    /// <summary>
    ///   Helper function to compute the `authHash`.
    /// </summary>
    private static string ComputeAuthHash(Account account, string ip)
    {
        string authHashComputeString = account.AccountId + ip + account.Cookie + _authHashMagic;
        return Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(authHashComputeString))).ToLowerInvariant();
    }

    private static Dictionary<int, Dictionary<int, BuddyListEntry>> BuddyListForAccount(Account account, FriendInfo[] friends)
    {
        Dictionary<int, BuddyListEntry> friendList = new();
        foreach (FriendInfo friend in friends)
        {
            friendList[friend.AccountId] = new(friend.NameWithClanTag, friend.AccountId, status: 2, standing: 3, clanTag: friend.ClanTag ?? "", friend.Group);
        }
        return new Dictionary<int, Dictionary<int, BuddyListEntry>>() { [account.AccountId] = friendList };
    }

    private static async Task<Dictionary<int, List<IgnoredListEntry>>> IgnoredListForAccount(Account account, BountyContext bountyContext)
    {
        IEnumerable<int> ignoredUserIds = account.IgnoredList.Select(accountId => int.Parse(accountId));
        Dictionary<int, List<IgnoredListEntry>> ignoredList = new()
        {
            [account.AccountId] = await bountyContext.Accounts.Where(a => ignoredUserIds.Contains(a.AccountId)).Select(a => new IgnoredListEntry(a.AccountId, a.Name)).ToListAsync()
        };
        return ignoredList;
    }

    private static Dictionary<int, List<BannedListEntry>> BannedListForAccount(Account account)
    {
        // TODO: Implement banned list properly.
        // NOTE: Game client crashes if BannedList is not specified.at all.
        return new Dictionary<int, List<BannedListEntry>>() { { account.AccountId, new List<BannedListEntry>() } };
    }

    public static List<Info> InfoForAccount(AccountDetails accountDetails, float tournamentRatingForActiveTeam)
    {
        Info info = new()
        {
            AccountId = accountDetails.AccountId.ToString(),
            Standing = "3",
            Level = "1",
            LevelExp = "0",
            // AllTimeTotalDisconnects: appears to be ignored
            // PossibleDisconnects: appears to be ignored
            // AllTimeGamesPlayed: appears to be ignored
            // NumBotGamesWon: appears to be ignored
            PSR = accountDetails.PublicRating,
            // PublicGameWins = publicStats.Wins,
            // PublicGameLosses = publicStats.Losses,
            PublicGamesPlayed = accountDetails.PublicGamesPlayed,
            PublicGameDisconnects = accountDetails.PublicTimesDisconnected,
            // NormalRankedGamesMMR: unused in KONGOR
            // NormalRankedGameWins: unused in KONGOR
            // NormalRankedGameLosses: unused in KONGOR
            // NormalRankedGamesPlayed: unused in KONGOR
            // NormalRankedGameDisconnects: unused in KONGOR
            // CasualModeMMR: unused in KONGOR
            // CasualModeWins: unused in KONGOR
            // CasualModeLosses: unused in KONGOR
            // CasualModeGamesPlayed: unused in KONGOR
            // CasualModeDisconnects: unused in KONGOR
            MidWarsMMR = accountDetails.MidWarsRating,
            MidWarsGamesPlayed = accountDetails.MidWarsGamesPlayed,
            MidWarsTimesDisconnected = accountDetails.MidWarsTimesDisconnected,

            // Number of Tournament matches played. Note: rift wars is used as a piggy-back.
            RiftWarsGamesPlayed = accountDetails.TournamentGamesPlayed,
            RiftWarsDisconnects = accountDetails.TournamentTimesDisconnected,
            RiftWarsRating = tournamentRatingForActiveTeam,

            IsNew = 0,
            ChampionsOfNewerthNormalMMR = accountDetails.CoNNormalRating,
            ChampionsOfNewerthNormalRank = accountDetails.CoNNormalRank,
            ChampionsOfNewerthGamesPlayed = accountDetails.CoNNormalGamesPlayed,
            ChampionsOfNewerthGameDisconnects = accountDetails.CoNNormalTimesDisconnected,

            ChampionsOfNewerthCasualMMR = accountDetails.CoNCasualRating,
            ChampionsOfNewerthCasualRank = accountDetails.CoNCasualRank,
            ChampionsOfNewerthCasualGamesPlayed = accountDetails.CoNCasualGamesPlayed,
            ChampionsOfNewerthCasualGameDisconnects = accountDetails.CoNCasualTimesDisconnected,

            // Additional Public Games info requested by server_requester.php?f=c_conn
            // Unclear if used or not.
            // PublicHeroKills = account.PlayerSeasonStatsPublic.HeroKills,
            // PublicHeroAssists = account.PlayerSeasonStatsPublic.HeroAssists,
            // PublicDeaths = account.PlayerSeasonStatsPublic.Deaths,
            // PublicWardsPlaced = account.PlayerSeasonStatsPublic.Wards,
            // PublicGoldEarned = account.PlayerSeasonStatsPublic.Gold,
            // PublicExpEarned = account.PlayerSeasonStatsPublic.Exp,
            // PublicSecondsPlayed = account.PlayerSeasonStatsPublic.Secs,
            // PublicTimeEarningExp = account.PlayerSeasonStatsPublic.TimeEarningExp,

            // Additional TMM info requested by server_requester.php?f=c_conn

            // Additional unknown fields requested by server_requester.php?f=c_conn
            // Unclear if used or not.
            Rnk_amm_solo_conf = 0,
            Rnk_amm_team_conf = 0,
        };

        return new List<Info>() { info };
    }

    private static ClanMemberInfo ClanMemberInfoForAccount(AccountDetailsForLogin account)
    {
        ClanMemberInfo clanMemberInfo;

        if (account.ClanId != null)
        {
            clanMemberInfo = new ClanMemberInfo
            {
                ClanId = account.ClanId.ToString(),
                AccountId = account.AccountId.ToString(),
                Rank = Clan.GetClanTierName(account.Account.ClanTier),
                Message = "",
                JoinDate = DateTime.UtcNow.ToString(),
                Name = account.ClanName,
                Tag = account.ClanTag,
                Creator = account.AccountId.ToString(),
                Title = "Clan " + account.ClanName,
                Active = "1",
                Logo = "0",
                IdleWarning = "0",
                ActiveIndex = "0"
            };
        }

        else
        {
            // NOTE: I don't believe this error string is used in the UI, but it matches what is in the actual HoN response.
            clanMemberInfo = new ClanMemberInfoError();
        }

        return clanMemberInfo;
    }
    
    private static async Task<Dictionary<int, ClanRosterEntry>> ClanRosterForClan(int clanId, BountyContext bountyContext)
    {
        return await bountyContext.Accounts.Where(acc => acc.Clan!.ClanId == clanId).Select(acc => new ClanRosterEntry()
            {
                AccountId = acc.AccountId,
                ClanId = acc.Clan!.ClanId,
                JoinDate = DateTime.UtcNow.ToString(), // TODO: fixme
                Message = "",
                Nickname = acc.Name,
                Rank = Clan.GetClanTierName(acc.ClanTier),
                Standing = acc.ClanTier.ToString()
            }).ToDictionaryAsync(entry => entry.AccountId, entry => entry);
    }

    private static Dictionary<string, object> MyUpgradesInfoForAccount(AccountDetailsForLogin accountDetails)
    {
        Dictionary<string, object> myUpgradesInfo = accountDetails.UnlockedUpgradeCodes
            .Where(upgrade => upgrade.StartsWith("ma.").Equals(false) && upgrade.StartsWith("cp.").Equals(false))
            .ToDictionary<string, string, object>(upgrade => upgrade, upgrade => new MyUpgradesInfoEntry());

#if false
        foreach (string boost in GameConsumables.GetOwnedMasteryBoostProducts(accountDetails.UnlockedUpgradeCodes))
            myUpgradesInfo.Add(boost, new MyUpgradesInfoEntry());

        foreach (KeyValuePair<string, Coupon> coupon in GameConsumables.GetOwnedCoupons(accountDetails.UnlockedUpgradeCodes))
            myUpgradesInfo.Add(coupon.Key, coupon.Value);
#endif

        return myUpgradesInfo;
    }

    private static CloudStorageInfo CloudStorageInfoForAccount(int accountId, string? useCloud, string? cloudAutoupload, string? fileModifyTime)
    {
        return new CloudStorageInfo(accountId, useCloud, cloudAutoupload, fileModifyTime);
    }
}
