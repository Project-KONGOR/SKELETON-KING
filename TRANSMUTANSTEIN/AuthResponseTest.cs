namespace TRANSMUTANSTEIN;

[TestClass]
public class AuthResponseTest
{
    [TestMethod]
    public void TestAuthResponseConstruction()
    {
        AccountDetails accountDetails = new(
            accountId: 3,
            accountName: "SouL",
            accountType: AccountType.Legacy,
            selectedUpgradeCodes: new List<string>()
            {
                "ai.Default Icon",
                "cc.white",
                "t.Standard",
            },

            autoConnectChatChannels: new List<string>()
            {
                "PBT",
            },
            ignoredAccountIds: new List<string>()
            {
                "1",
            },
            accountStats: new AccountStats(
                level: 1,
                levelExp: 2.5f,
                psr: 1500,
                normalRankedGamesMMR: 1650.2f,
                casualModeMMR: 1534.1f,
                publicGamesPlayed: 12,
                normalRankedGamesPlayed: 13,
                casualModeGamesPlayed: 14,
                midWarsGamesPlayed: 15,
                allOtherGamesPlayed: 16,
                publicGameDisconnects: 3,
                normalRankedGameDisconnects: 4,
                casualModeDisconnects: 5,
                midWarsTimesDisconnected: 6,
                allOtherGameDisconnects: 7),

            // Clan information.
            clanId: null,
            clanName: null,
            clanTag: null,
            clanTier: Clan.Tier.None,

            // CloudStorage.
            useCloud: false,
            cloudAutoUpload: false,

            // User-specific information.
            userId: "0868545f-4d35-43d0-b8e5-ab6ecbc497ed",
            email: "SouL@gmail.com",
            goldCoins: 9999874,
            silverCoins: 9993099,
            unlockedUpgradeCodes: new List<string>()
            {
                "ai.Default Icon",
                "cc.white",
                "t.Standard",
                "aa.Hero_Pearl.Alt10",
                "aa.Hero_Andromeda.Alt6",
                "aa.Hero_MasterOfArms.Alt5",
                "aa.Hero_MasterOfArms.Alt2",
                "aa.Hero_Rampage.Alt13",
                "aa.Hero_SirBenzington.Alt11",
            }
        )
        {
            Identities = new List<List<string>>()
            {
                new List<string>()
                {
                    "SouL", "3"
                }
            },
            BuddyList = new Dictionary<int, BuddyListEntry>()
            {
                [175] = new BuddyListEntry("kokoJambo", 175, "", ""),
                [184385] = new BuddyListEntry("L4YTENN", 184385, "", ""),
                [1164] = new BuddyListEntry("FalseGreek", 1164, "", ""),
                [63985] = new BuddyListEntry("Astra", 63985, "", ""),
                [2087] = new BuddyListEntry("DeepLove", 2087, "", ""),
                [29234] = new BuddyListEntry("Loopish", 29234, "", ""),
                [38615] = new BuddyListEntry("Mr-White", 38615, "", ""),
                [77635] = new BuddyListEntry("TheReceiver", 77635, "", ""),
                [1034] = new BuddyListEntry("Vulka", 1034, "", ""),
                [91574] = new BuddyListEntry("WhaT_YoU_GoT", 91574, "", ""),
                [467] = new BuddyListEntry("gjanko", 467, "", ""),
                [8327] = new BuddyListEntry("SALT", 8327, "", ""),
                [72190] = new BuddyListEntry("julle", 72190, "", ""),
                [151525] = new BuddyListEntry("`HAHAHAHAHA`", 151525, "", ""),
                [17737] = new BuddyListEntry("Blore", 17737, "", ""),
                [152] = new BuddyListEntry("HON_OMG", 152, "", ""),
                [611] = new BuddyListEntry("Kragiko", 611, "", ""),
                [5070] = new BuddyListEntry("Nathill", 5070, "", ""),
                [243016] = new BuddyListEntry("XGod", 243016, "", ""),
            },
            Notifications = new List<NotificationEntry>()
            {
                new NotificationEntry("kokoJambo||2|notify_buddy_added|notfication_generic_action|action_friend_request|4/17/2023 2:16:37 PM|0", 3),
                new NotificationEntry("L4YTENN||2|notify_buddy_added|notfication_generic_action|action_friend_request|4/17/2023 5:59:39 PM|0", 9),
                new NotificationEntry("Astra||2|notify_buddy_added|notfication_generic_action|action_friend_request|4/17/2023 6:03:14 PM|0", 15),
                new NotificationEntry("DeepLove||2|notify_buddy_added|notfication_generic_action|action_friend_request|4/17/2023 6:03:17 PM|0", 18),
                new NotificationEntry("Loopish||2|notify_buddy_added|notfication_generic_action|action_friend_request|4/17/2023 6:03:18 PM|0", 21),
                new NotificationEntry("Mr-White||2|notify_buddy_added|notfication_generic_action|action_friend_request|4/17/2023 6:03:21 PM|0", 24),
                new NotificationEntry("FalseGreek||2|notify_buddy_added|notfication_generic_action|action_friend_request|4/17/2023 6:03:21 PM|0", 26),
                new NotificationEntry("TheReceiver||2|notify_buddy_added|notfication_generic_action|action_friend_request|4/17/2023 6:03:22 PM|0", 28),
                new NotificationEntry("Vulka||2|notify_buddy_added|notfication_generic_action|action_friend_request|4/17/2023 6:15:43 PM|0", 57),
                new NotificationEntry("WhaT_YoU_GoT||2|notify_buddy_added|notfication_generic_action|action_friend_request|4/17/2023 6:16:01 PM|0", 61),
                new NotificationEntry("gjanko||2|notify_buddy_added|notfication_generic_action|action_friend_request|4/17/2023 6:45:49 PM|0", 72),
                new NotificationEntry("SALT||2|notify_buddy_added|notfication_generic_action|action_friend_request|4/17/2023 7:00:57 PM|0", 79),
                new NotificationEntry("julle||2|notify_buddy_added|notfication_generic_action|action_friend_request|4/17/2023 8:03:42 PM|0", 86),
                new NotificationEntry("`HAHAHAHAHA`||2|notify_buddy_added|notfication_generic_action|action_friend_request|4/21/2023 8:21:56 PM|0", 126),
                new NotificationEntry("Blore||2|notify_buddy_added|notfication_generic_action|action_friend_request|4/21/2023 8:21:59 PM|0", 130),
                new NotificationEntry("HON_OMG||2|notify_buddy_added|notfication_generic_action|action_friend_request|4/21/2023 8:22:00 PM|0", 133),
                new NotificationEntry("Kragiko||2|notify_buddy_added|notfication_generic_action|action_friend_request|4/21/2023 8:22:01 PM|0", 136),
                new NotificationEntry("XGod||2|notify_buddy_added|notfication_generic_action|action_friend_request|4/21/2023 8:25:13 PM|0", 139),
                new NotificationEntry("Nathill||2|notify_buddy_added|notfication_generic_action|action_friend_request|4/21/2023 8:25:44 PM|0", 141),

            },
            IgnoredList = new List<IgnoredListEntry>()
            {
                new IgnoredListEntry(1, "DEVOURER")
            }
        };

        string cookie = "C81C1CDA25B2F0E600681167BCC3D446";
        string clientIpAddress = "127.0.0.1";
        long hostTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        string chatServerUrl = "192.168.0.1";
        string icbUrl = "www.google.com";

        AuthResponse authResponse = new(accountDetails, cookie, clientIpAddress, hostTime, chatServerUrl, icbUrl, new("initial_vector", "hash_code", "key_version"));
        Assert.AreEqual(accountDetails.AccountId, authResponse.AccountId);
        Assert.AreEqual(accountDetails.AccountType, authResponse.AccountType);
        Assert.AreEqual(false, authResponse.AccountCloudStorageInfo.UseCloud);
        Assert.AreEqual(false, authResponse.AccountCloudStorageInfo.CloudAutoUpload);
        Assert.AreEqual(accountDetails.AccountStats, authResponse.AccountStats.First());
        Assert.AreSame(accountDetails.SelectedUpgradeCodes, authResponse.SelectedUpgrades);
        Assert.AreEqual(accountDetails.AccountName, authResponse.Nickname);
        Assert.AreEqual(accountDetails.Email, authResponse.Email);
        Assert.AreEqual(clientIpAddress, authResponse.ClientIpAddress);
        Assert.AreEqual(cookie, authResponse.Cookie);
        Assert.AreEqual("82d5a30da745ff372d17434b683a5e9b2db8f0ea", authResponse.AuthHash);
        Assert.AreEqual(hostTime, authResponse.HostTime);
        Assert.AreEqual(5, authResponse.VestedThreshold);
        Assert.AreEqual(chatServerUrl, authResponse.ChatUrl);
        Assert.AreEqual(null, authResponse.ClanMemberInfo);
        Assert.AreEqual(0.05f, authResponse.LeaverThreashold);
        Assert.AreEqual(accountDetails.IgnoredList, authResponse.IgnoredList[accountDetails.AccountId]);
        Assert.AreEqual(accountDetails.ClanRoster, authResponse.ClanRoster);
        Assert.AreEqual(accountDetails.Identities, authResponse.Identities);
        Assert.AreEqual(accountDetails.BuddyList, authResponse.BuddyList[accountDetails.AccountId]);
        Assert.AreEqual(accountDetails.AutoConnectChatChannels, authResponse.ChatRooms);
        Assert.AreEqual(accountDetails.Notifications, authResponse.Notifications);
        Assert.AreEqual(accountDetails.GoldCoins, authResponse.GoldCoins);
        Assert.AreEqual(accountDetails.SilverCoins, authResponse.SilverCoins);
        Assert.AreEqual(icbUrl, authResponse.IcbUrl);
        Assert.AreEqual("initial_vector", authResponse.SecInfo.InitialVector);
        Assert.AreEqual("hash_code", authResponse.SecInfo.HashCode);
        Assert.AreEqual("key_version", authResponse.SecInfo.KeyVersion);
    }
}
