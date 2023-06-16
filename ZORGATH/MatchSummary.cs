namespace ZORGATH;

/// <summary>
///     The match summary which is part of the `client_requester.php?f=get_match_stats` response.
///     
///     NOTE: I used this as a guide to decode a lot of the game options abbreviations -
///     https://hon.fandom.com/wiki/Game_Modes#Game_Options
/// </summary>
public class MatchSummary
{
    // TODO: Add the missing fields to `matchResults` and propagate them here.
    public MatchSummary(MatchResults matchResults, int winningTeam, string replayServerUrl)
    {
        MatchId = matchResults.match_id.ToString();
        // GameServerId
        Map = matchResults.map;
        // MapVersion
        TimePlayed = matchResults.time_played.ToString();
        // FileSize
        // FileName
        // CState
        Version = matchResults.version;
        // AveragePsr
        Date = matchResults.date;
        Time = matchResults.time;
        // MatchName
        // Class
        // Private
        // NormalMode (nm)
        // SingleDraft (sd)
        // RandomDraft (rd)
        // dm
        // BanningDraft (bd)
        // BanningPick (bp)
        // AllRandom (ar)
        // cd
        // cm
        // LockPick (lp)
        // BlindBan (bb)
        // BlitzMode (bm)
        // km
        // League
        // MaxPlayers
        // Tier
        // NoRepick
        // NoAgilityHeroes
        // DropItem 
        // NoRespawnTimer
        // ReverseHeroSelect
        // NoHeroSwap
        // NoIntelHeroes
        // AltHeroPicking
        // Veto
        // ShuffleTeam
        // NoStrengthHeroes
        // NoPowerUps
        // DuplicateHeroes
        // AllPick
        // BalancedRandom
        // EasyMode
        // cas
        // rs
        // nl
        // OfficialMatch
        // NoStats
        // AutoBalanced (ab)
        // Hardcore
        // DeveloperHeroes
        // VerifiedOnly
        // Gated
        // RapidFire
        Timestamp = matchResults.timestamp;  // NOTE: This is used for showing RAP button.
        // LegacyReplayUrl
        Size = Math.Max(matchResults.file_size, 1); // Make sure file size is > 0.
        ServerName = matchResults.name;
        // ReplayDirectory (dir)

        ReplayUrl = $"{replayServerUrl}/replays/M{matchResults.match_id}.honreplay";
        WinningTeam = winningTeam;
        GameMode = matchResults.gamemode!;
        MostValuablePlayer = matchResults.mvp.ToString();
        MostAnnihilationsAward = matchResults.awd_mann.ToString();
        MostQuadKillsAward = matchResults.awd_mqk.ToString();
        BestKillStreakAward = matchResults.awd_lgks.ToString();
        MostSmackdownsAward = matchResults.awd_msd.ToString();
        MostHeroKillsAward = matchResults.awd_msd.ToString();
        MostAssistsAward = matchResults.awd_masst.ToString();
        LeastDeathAward = matchResults.awd_ledth.ToString();
        MostBuildingDamageAward = matchResults.awd_mbdmg.ToString();
        MostWardKillsAward = matchResults.awd_mwk.ToString();
        MostHeroDamageAward = matchResults.awd_mhdd.ToString();
        HighestCreepScoreAward = matchResults.awd_hcs.ToString();
    }

    /// <summary>
    ///     The ID for the match.
    /// </summary>
    [PhpProperty("match_id")]
    public string? MatchId { get; set; }

    /// <summary>
    ///     The ID for the server.
    /// </summary>
    // [PhpProperty("server_id")]
    // public string? GameServerId { get; set; }

    /// <summary>
    ///     The name of the map. E.g. "caldavar".
    /// </summary>
    [PhpProperty("map")]
    public string? Map { get; set; }

    /// <summary>
    ///     Unknown. Some kind of string indicating the version of the map.
    ///     For Caldavar it seems to be "0.0.0"
    /// </summary>
    // [PhpProperty("map_version")]
    // public string? MapVersion { get; set; }

    /// <summary>
    ///     The length of the game. (I believe it's in seconds but haven't confirmed.)
    /// </summary>
    [PhpProperty("time_played")]
    public string? TimePlayed { get; set; }

    /// <summary>
    ///     The base URL for the location where the replay files are stored.
    /// </summary>
    // [PhpProperty("file_host")]
    // public string? FileHost { get; set; }

    /// <summary>
    ///     Unknown. This seems to be `0` even on the official servers which had
    ///     replays. Maybe this was the bytes of the file size at one point?
    /// </summary>
    // [PhpProperty("file_size")]
    // public string? FileSize { get; set; }

    /// <summary>
    ///     The path to the replay file itself. E.g. "~/replays/163277682.honreplay".
    /// </summary>
    // [PhpProperty("file_name")]
    // public string? FileName { get; set; }

    /// <summary>
    ///     Unknown.
    /// </summary>
    // [PhpProperty("c_state")]
    // public string? CState { get; set; }

    /// <summary>
    ///     The game version when this replay was played. E.g. "4.10.1.0".
    /// </summary>
    [PhpProperty("version")]
    public string? Version { get; set; }

    /// <summary>
    ///     Unconfirmed -- the average PSR of the players in the match. For TMM
    ///     this value was just `0`.
    /// </summary>
    // [PhpProperty("avgpsr")]
    // public string? AveragePsr { get; set; }

    /// <summary>
    ///     The date the game was played in US format (month/day/year). E.g. "4/10/2022"
    /// </summary>
    [PhpProperty("date")]
    public string? Date { get; set; }

    /// <summary>
    ///     The time when the match was played, in the format (hour:minute:seconds AM/PM). E.g. "01:26:26 PM".
    /// </summary>
    [PhpProperty("time")]
    public string? Time { get; set; }

    /// <summary>
    ///     The name of the match. E.g. "TMM Match #163277682"
    /// </summary>
    [PhpProperty("mname")]
    public string? MatchName { get; set; }

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("class")]
    public string? Class { get; set; }

    /// <summary>
    ///     Unknown.
    /// </summary>
    // [PhpProperty("private")]
    // public string? Private { get; set; }

    /// <summary>
    ///      Unconfirmed -- A string boolean ("0" or "1") indicating if the match is a normal mode match.
    /// </summary>
    [PhpProperty("nm")]
    public string NormalMode { get; set; } = "0";

    /// <summary>
    ///      Unconfirmed -- A string boolean ("0" or "1") indicating if the match is a single draft match.
    /// </summary>
    [PhpProperty("sd")]
    public string SingleDraft { get; set; } = "0";

    /// <summary>
    ///      Unconfirmed -- A string boolean ("0" or "1") indicating if the match is a random draft match.
    /// </summary>
    [PhpProperty("rd")]
    public string RandomDraft { get; set; } = "0";

    /// <summary>
    ///     Unknown. A string boolean ("0" or "1") value.
    /// </summary>
    [PhpProperty("dm")]
    public string Dm { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if the match is a banning draft match.
    /// </summary>
    [PhpProperty("bd")]
    public string BanningDraft { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if the match is a banning draft match.
    /// </summary>
    [PhpProperty("bp")]
    public string BanningPick { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if the match is a banning draft match.
    /// </summary>
    [PhpProperty("ar")]
    public string AllRandom { get; set; } = "0";

    /// <summary>
    ///     Unknown. A string boolean ("0" or "1") value.
    /// </summary>
    // [PhpProperty("cd")]
    // public string Cd { get; set; } = "0";

    /// <summary>
    ///     Unknown. A string boolean ("0" or "1") value.
    /// </summary>
    [PhpProperty("cm")]
    public string Cm { get; set; } = "0";

    /// <summary>
    ///      Unconfirmed -- A string boolean ("0" or "1") indicating if the match is a normal mode match.
    /// </summary>
    [PhpProperty("lp")]
    public string LockPick { get; set; } = "0";

    /// <summary>
    ///      Unconfirmed -- A string boolean ("0" or "1") indicating if the match is a normal mode match.
    /// </summary>
    [PhpProperty("bb")]
    public string BlindBan { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if the match is a normal mode match.
    /// </summary>
    [PhpProperty("bm")]
    public string BlitzMode { get; set; } = "0";

    /// <summary>
    ///     Unknown. A string boolean ("0" or "1") value.
    /// </summary>
    [PhpProperty("km")]
    public string Km { get; set; } = "0";

    /// <summary>
    ///     Unknown.
    /// </summary>
    // [PhpProperty("league")]
    // public string League { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed - The maximum number of players allowed on a team for this game.
    ///     Typically "5".
    /// </summary>
    // [PhpProperty("max_players")]
    // public string MaxPlayers { get; set; } = "5";

    /// <summary>
    ///     Unknown.
    /// </summary>
    // [PhpProperty("tier")]
    // public string Tier { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if the match had No Hero Repick enabled.
    /// </summary>
    // [PhpProperty("no_repick")]
    // public string NoRepick { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if the match had No Agi Heroes enabled.
    /// </summary>
    // [PhpProperty("no_agi")]
    // public string NoAgilityHeroes { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if the match had Drop Item enabled.
    /// </summary>
    // [PhpProperty("drp_itm")]
    // public string DropItem { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if the match had No Respawn Timer enabled.
    /// </summary>
    // [PhpProperty("no_timer")]
    // public string NoRespawnTimer { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if the match had Reverse Hero Select enabled.
    /// </summary>
    // [PhpProperty("rev_hs")]
    // public string ReverseHeroSelect { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if the match had No Hero Swap enabled.
    /// </summary>
    // [PhpProperty("no_swap")]
    // public string NoHeroSwap { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if the match had No Intel Heroes enabled.
    /// </summary>
    // [PhpProperty("no_int")]
    // public string NoIntelHeroes { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if the match had Alt Hero Picking
    ///     (the snake-style picking that is common in banning pick) enabled.
    /// </summary>
    [PhpProperty("alt_pick")]
    public string AltHeroPicking { get; set; } = "0";

    /// <summary>
    ///     Unknown. A string boolean ("0" or "1") value.
    /// </summary>
    // [PhpProperty("veto")]
    // public string Veto { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if the match had Shuffle Team enabled.
    /// </summary>
    [PhpProperty("shuf")]
    public string ShuffleTeam { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if the match had No Str Heroes enabled.
    /// </summary>
    // [PhpProperty("no_str")]
    // public string NoStrengthHeroes { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if the match had No Power-Ups enabled.
    /// </summary>
    // [PhpProperty("no_pups")]
    // public string NoPowerUps { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if the match had Duplicate Heroes enabled.
    /// </summary>
    // [PhpProperty("dup_h")]
    // public string DuplicateHeroes { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if it is an All Pick match.
    /// </summary>
    [PhpProperty("ap")]
    public string AllPick { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if it is an Balanced Random match.
    /// </summary>
    [PhpProperty("br")]
    public string BalancedRandom { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if it is an Easy Mode match.
    /// </summary>
    [PhpProperty("em")]
    public string EasyMode { get; set; } = "0";

    /// <summary>
    ///     Unknown. A string boolean ("0" or "1") value.
    /// </summary>
    [PhpProperty("cas")]
    public string Cas { get; set; } = "0";

    /// <summary>
    ///     Unknown. A string boolean ("0" or "1") value.
    /// </summary>
    // [PhpProperty("rs")]
    // public string Rs { get; set; } = "0";

    /// <summary>
    ///     Unknown.
    /// </summary>
    [PhpProperty("nl")]
    public string Nl { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if the match is Official.
    /// </summary>
    // [PhpProperty("officl")]
    // public string OfficialMatch { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if the match has No Stats enabled.
    /// </summary>
    [PhpProperty("no_stats")]
    public string NoStats { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if the match has Auto Balanced enabled.
    /// </summary>
    [PhpProperty("ab")]
    public string AutoBalanced { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if the match has Hardcore enabled.
    /// </summary>
    // [PhpProperty("hardcore")]
    // public string Hardcore { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if the match has Developer Heroes enabled.
    ///     
    ///     NOTE: I believe this option was removed so it's not even in the game.
    /// </summary>
    // [PhpProperty("dev_heroes")]
    // public string DeveloperHeroes { get; set; } = "0";

    /// <summary>
    ///     Unconfirmed -- A string boolean ("0" or "1") indicating if the match has Verified Only enabled.
    /// </summary>
    [PhpProperty("verified_only")]
    public string VerifiedOnly { get; set; } = "0";

    /// <summary>
    ///     Unknown. A string boolean ("0" or "1") value.
    /// </summary>
    [PhpProperty("gated")]
    public string Gated { get; set; } = "0";

    /// <summary>
    ///     Unknown. A string boolean ("0" or "1") value.
    /// </summary>
    // [PhpProperty("rapidfire")]
    // public string RapidFire { get; set; } = "0";

    /// <summary>
    ///     The UTC epoch timestamp for the game.
    /// </summary>
    [PhpProperty("timestamp")]
    public long Timestamp { get; set; }

    /// <summary>
    ///     The legacy URL for the replay. E.g. "http://replaydl.heroesofnewerth.com/ovh2USE206/saves/replays/20220410/M163277682.honreplay"
    ///
    ///     NOTE: I don't believe this was used in modern version of HoN.
    /// </summary>
    [PhpProperty("url")]
    public string? LegacyReplayUrl { get; set; }

    /// <summary>
    ///     Unconfirmed -- The size in bytes of the replay file. It seems this is used instead of `FileSize` in modern HoN.
    /// </summary>
    [PhpProperty("size")]
    public int Size { get; set; }

    /// <summary>
    ///     The server name. E.g. "USE206".
    /// </summary>
    [PhpProperty("name")]
    public string? ServerName { get; set; }

    /// <summary>
    ///     The directory path for the replay file. E.g. "/saves/replays/20220410"
    ///
    ///     NOTE: I don't believe this was used in modern version of HoN, since the moved to s3.
    /// </summary>
    // [PhpProperty("dir")]
    // public string ReplayDirectory { get; set; }

    /// <summary>
    ///     The  URL for the replay. E.g. "http://s3.amazonaws.com/naeu-icb2/replays/20220410/ovh2USE206/M163277682.honreplay"
    ///
    ///     NOTE: I don't know if this needs to be an s3 URL or not.
    /// </summary>
    [PhpProperty("s3_url")]
    public string ReplayUrl { get; set; }

    /// <summary>
    ///     The winning team. "1" for legion and "2" for Hellbourne.
    /// </summary>
    [PhpProperty("winning_team")]
    public int WinningTeam { get; set; }

    /// <summary>
    ///     The abbrevated string for the game mode (e.g. "cp" for Captains Pick).
    /// </summary>
    [PhpProperty("gamemode")]
    public string GameMode { get; set; }

    /// <summary>
    ///     The account ID of the player who was vote MVP. Or -1 if none.
    /// </summary>
    [PhpProperty("mvp")]
    public string MostValuablePlayer { get; set; } = "-1";

    /// <summary>
    ///     The account ID of the player who had the Most Annihilations. Or -1 if none.
    /// </summary>
    [PhpProperty("awd_mann")]
    public string MostAnnihilationsAward { get; set; } = "-1";

    /// <summary>
    ///     The account ID of the player who had the Most Quad Kills. Or -1 if none.
    /// </summary>
    [PhpProperty("awd_mqk")]
    public string MostQuadKillsAward { get; set; } = "-1";

    /// <summary>
    ///     The account ID of the player who had the Best Kill Streak award. Or -1 if none.
    /// </summary>
    [PhpProperty("awd_lgks")]
    public string BestKillStreakAward { get; set; } = "-1";

    /// <summary>
    ///     The account ID of the player who had the Most Smackdowns award. Or -1 if none.
    /// </summary>
    [PhpProperty("awd_msd")]
    public string MostSmackdownsAward { get; set; } = "-1";

    /// <summary>
    ///     The account ID of the player who had the Most Hero Kills award. Or -1 if none.
    /// </summary>
    [PhpProperty("awd_mkill")]
    public string MostHeroKillsAward { get; set; } = "-1";

    /// <summary>
    ///     The account ID of the player who had the Most Assists award. Or -1 if none.
    /// </summary>
    [PhpProperty("awd_masst")]
    public string MostAssistsAward { get; set; } = "-1";

    /// <summary>
    ///     The account ID of the player who had the Least Deaths award. Or -1 if none.
    /// </summary>
    [PhpProperty("awd_ledth")]
    public string LeastDeathAward { get; set; } = "-1";

    /// <summary>
    ///     The account ID of the player who had the Most Building Damage award. Or -1 if none.
    /// </summary>
    [PhpProperty("awd_mbdmg")]
    public string MostBuildingDamageAward { get; set; } = "-1";

    /// <summary>
    ///     The account ID of the player who had the Most Ward Kills award. Or -1 if none.
    ///     
    ///     NOTE: I don't believe this is actually used in the Lua file.
    /// </summary>
    [PhpProperty("awd_mwk")]
    public string MostWardKillsAward { get; set; } = "-1";

    /// <summary>
    ///     The account ID of the player who had the Most Hero Damage award. Or -1 if none.
    ///     
    ///     NOTE: I don't believe this is actually used in the Lua file.
    /// </summary>
    [PhpProperty("awd_mhdd")]
    public string MostHeroDamageAward { get; set; } = "-1";

    /// <summary>
    ///     The account ID of the player who had the Highest Creep Score. Or -1 if none.
    /// </summary>
    [PhpProperty("awd_hcs")]
    public string HighestCreepScoreAward { get; set; } = "-1";

    // The following properties appear to be supported too
    // "bots"
    // "fp"
    // "sm"
    // "mwb"

    [PhpProperty("mmpoints")]
    public string MMPoints { get; set; } = "7"; // for testing

    // "selected_upgrades"
}
