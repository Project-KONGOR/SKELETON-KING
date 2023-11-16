namespace KINESIS.GameFinder;

public class PendingMatch
{
    public readonly List<TMMGroup> Legion;
    public readonly List<TMMGroup> Hellbourne;
    public readonly string GameMode;
    public readonly TMMGameType GameType;
    public readonly int TeamSize;

    public PendingMatch(List<TMMGroup> legion, List<TMMGroup> hellbourne, string gameMode, TMMGameType gameType, int teamSize)
    {
        Legion = legion;
        Hellbourne = hellbourne;
        GameMode = gameMode;
        GameType = gameType;
        TeamSize = teamSize;
    }

    //    public readonly ConnectedServer PendingServer;
    //    public readonly long CreationTime;
    //    public readonly int PendingMatchId;
    //    public readonly int CreationGeneration;
    //    public readonly string MatchSettings;
    //    public readonly string MapName;
    //    public ServerInfo? ServerInfo = null;
    //    public bool? IsServerPortReachable = null;

    //    public float Disparity => Math.Abs(Legion.TeamMMR() - Hellbourne.TeamMMR());
    /*
    public PendingMatch(int pendingMatchId, int generation, List<TMMGroup> legion, List<TMMGroup> hellbourne, string gameMode, string mapName, TMMGameType gameType, int teamSize, ConnectedServer pendingServer, long now)
    {
        PendingMatchId = pendingMatchId;
        CreationGeneration = generation;
        Legion = legion;
        Hellbourne = hellbourne;
        PendingServer = pendingServer;
        GameMode = gameMode;
        GameType = gameType;
        CreationTime = now;
        TeamSize = teamSize;
        MatchSettings = CreateMatchSettings(gameMode, mapName, teamSize);
        MapName = mapName;
    }

    public PendingMatch(int pendingMatchId, int generation, List<TMMGroup> legion, List<TMMGroup> hellbourne, ConnectedServer pendingServer, string matchSettings, int teamSize, long now)
    {
        PendingMatchId = pendingMatchId;
        CreationGeneration = generation;
        Legion = legion;
        Hellbourne = hellbourne;
        PendingServer = pendingServer;

        Dictionary<string, string> individualMatchSettings = matchSettings.Split(' ').Select(x => x.Split(':')).ToDictionary(x => x[0], x => x[1]);
        individualMatchSettings.TryAdd("mode", "rb");
        GameMode = individualMatchSettings["mode"];

        individualMatchSettings.TryAdd("teamsize", teamSize.ToString());
        TeamSize = int.Parse(individualMatchSettings["teamsize"]);

        individualMatchSettings.TryAdd("map", "caldavar");
        MapName = individualMatchSettings["map"];

        GameType = TMMGameType.PUBLIC;
        CreationTime = now;
        MatchSettings = string.Join(' ', individualMatchSettings.Select(x => x.Key + ":" + x.Value));
    }
    */
}
