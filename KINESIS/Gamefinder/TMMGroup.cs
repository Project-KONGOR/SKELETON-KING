using KINESIS.Matchmaking;

namespace KINESIS.GameFinder;

public class TMMGroup
{
    //public readonly List<MatchmakingGroupPlayerInfo> AccountInfo;
    public readonly float AdjustedGroupMMR;
    public readonly float AdjustedAverageMMR;
    public readonly float LowestMMR;
    public readonly float HighestMMR;
    public readonly bool TopOfTheQueue;
    public readonly HashSet<string> Regions;
    public readonly TMMGameType GameType;
    public readonly HashSet<string> GameModes;
    public readonly MatchmakingGroup MatchmakingGroup;
    // public int ApproximateCompatiblePoolSize = 0;
    public readonly long TimestampWhenJoinedQueue;
    
    // public bool IsSoloQ => AccountInfo.Count == 1;
    // public List<KeyValuePair<int, int>>? SortedPingInformation;
    // public Dictionary<int, int>? PingInformation;
    // public int PingThreshold;
    // public int PositionInQueue = 0;
    // public bool IsReserved = false;
    // public string Status = "Not Added To Queue Yet";
    public GameFinder.PendingMatch? PendingMatch = null;

    public TMMGroup(int groupSize, float groupMMR, float lowestMMR, float highestMMR, bool topOfTheQueue, HashSet<string> regions, TMMGameType gameType, HashSet<string> gameModes, MatchmakingGroup matchmakingGroup, long timestampWhenJoinedQueue)
    {
        if (groupSize == 1)
        {
            AdjustedGroupMMR = groupMMR;
        }
        else
        {
            AdjustedGroupMMR = groupMMR + groupSize * 30;
        }

        AdjustedAverageMMR = AdjustedGroupMMR / groupSize;
        LowestMMR = lowestMMR;
        HighestMMR = highestMMR;
        TopOfTheQueue = topOfTheQueue;
        Regions = regions;
        GameType = gameType;
        GameModes = gameModes;
        MatchmakingGroup = matchmakingGroup;
        TimestampWhenJoinedQueue = timestampWhenJoinedQueue;
        

        /*
        //AccountInfo = accountInfo;
        Regions = regions;
        GameType = gameType;
        GameModes = gameModes;
        MatchmakingGroup = matchmakingGroup;
        TimestampWhenJoinedQueue = timestampWhenJoinedQueue;
        IsLookingForPlacementMatch = isLookingForPlacementMatch;

        PingInformation = pingInformation;
        if (pingInformation != null)
        {
            SortedPingInformation = pingInformation.ToList();
            SortedPingInformation.Sort((a, b) => a.Value.CompareTo(b.Value));
        }
        else
        {
            SortedPingInformation = null;
        }
        PingThreshold = pingThreshold;

        // Scale up group MMR so that 2-3-4-5 man queues are balanced more fairly against solo q players.
        if (accountInfo.Count != 1)
        {
            groupMMR *= 1 + accountInfo.Count / 500.0f;
        }

        AdjustedGroupMMR = groupMMR;
        AdjustedAverageMMR = AdjustedGroupMMR / accountInfo.Count;
        LowestMMR = lowestMMR;
        HighestMMR = highestMMR;
        TopOfTheQueue = topOfTheQueue;
        ProtectedGroup = protectedGroup;

        TournamentMode = tournamentMode;
        TournamentMMR = tournamentMMR;
        */
    }
}
