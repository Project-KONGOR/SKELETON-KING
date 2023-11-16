namespace KINESIS.GameFinder;

public class ConnectedServer
{
    public string Name = "ConnectedServer";
    public PendingMatch? PendingMatch = null;

    public void Disconnect(string reason)
    {

    }
}

public class GameFinder
{
    internal record MatchUp(List<TMMGroup> Legion, List<TMMGroup> Hellbourne, float Disparity);

    // Collections for adding and deleting TMMGroups to and from the queue.
    // Having these separate collections avoid locking the entire GameFinder to add/delete elements.
    private static ConcurrentQueue<TMMGroup> GroupsToAdd = new();
    private static ConcurrentQueue<TMMGroup> GroupsToRemove = new();

    private static int _nextPendingMatchId = 0;

    // public static string MostRecentTMMStats = "";

    internal static readonly Comparison<TMMGroup> SortByTimeSpentInQueue = (TMMGroup a, TMMGroup b) =>
    {
        bool aTopOfTheQueue = a.TopOfTheQueue;
        bool bTopOfTheQueue = b.TopOfTheQueue;
        if (aTopOfTheQueue != bTopOfTheQueue)
        {
            return -aTopOfTheQueue.CompareTo(bTopOfTheQueue);
        }
        return a.TimestampWhenJoinedQueue.CompareTo(b.TimestampWhenJoinedQueue);
    };

    public record ServerInfo(int MatchId, string Address, short Port);

    private static ServerInfo FailedServerInfo = new ServerInfo(0, "", 0);

    public class PendingMatch
    {
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

        // Only used for debugging.
        public override string ToString()
        {
            return "L: " + string.Join(",", Legion) + "; H: " + string.Join(",", Hellbourne);
        }

        public void SetStatus(string status)
        {
            Status = status;

            string groupStatus = "In Pending Match " + PendingMatchId + " " + status;
            foreach (TMMGroup tmmGroup in BothTeams)
            {
                tmmGroup.Status = groupStatus;
            }
        }

        */
        public readonly List<TMMGroup> Legion;
        public readonly List<TMMGroup> Hellbourne;
        /*        public IEnumerable<TMMGroup> BothTeams => Legion.Concat(Hellbourne);
        public readonly string GameMode;
        public readonly TMMGameType GameType;
        public readonly int TeamSize;
        */
        public readonly ConnectedServer PendingServer;
        public readonly long CreationTime;
        /*
        public readonly int CreationGeneration;
        public readonly string MatchSettings;
        public readonly string MapName;
        public string Status = "Just created";
        */

        public readonly int PendingMatchId = Interlocked.Increment(ref _nextPendingMatchId);
        public ServerInfo? ServerInfo = null;

        public bool? IsServerPortReachable = null;
        /*
        public float Disparity => Math.Abs(Legion.TeamMMR() - Hellbourne.TeamMMR());
        public bool ProtectedMatch => Legion.Any(g => g.ProtectedGroup) || Hellbourne.Any(g => g.ProtectedGroup);
        */
    }

    private readonly List<TMMGroup> _tmmGroups = new();
    private readonly List<PendingMatch> _pendingMatches = new();
    private readonly Random _random = new Random();
    private Timer _timer;

    public GameFinder()
    {
        _timer = new(TimerCallback, this, Timeout.Infinite, Timeout.Infinite);
    }
    public void Start()
    {
        RearmTimer();
    }

    public static void AddToQueue(TMMGroup tmmGroup)
    {
        if (tmmGroup == null)
        {
            Console.WriteLine("Attempted to add null TMMGroup to the GroupsToAdd.");
            return;
        }

        // TODO: verify that none of the players in the TMMGroup are in any of the other TMMGroups?
        GroupsToAdd.Enqueue(tmmGroup);
    }

    public static void RemoveFromQueue(TMMGroup tmmGroup)
    {
        GroupsToRemove.Enqueue(tmmGroup);
    }

    internal void FindGames(long now)
    {
        // Must always add first in case the group was added and then immediately removed within one iteration.
        while (GroupsToAdd.TryDequeue(out var groupToAdd))
        {
            if (groupToAdd == null)
            {
                Console.WriteLine("Attempted to add null TMMGroup to the _tmmGroups.");
                continue;
            }
            _tmmGroups.Add(groupToAdd);
        }

        while (GroupsToRemove.TryDequeue(out var groupToRemove))
        {
            if (groupToRemove == null)
            {
                Console.WriteLine("Attempted to remove null TMMGroup from _tmmGroups.");
                continue;
            }
            if (groupToRemove.PendingMatch != null)
            {
                // Mark as failed. It will be removed and disbanded during ProcessPendingMatches().
                groupToRemove.PendingMatch.ServerInfo = FailedServerInfo;
            }
            else if (!_tmmGroups.Remove(groupToRemove))
            {
                Console.WriteLine("Attempted to remove an unknown TMMGroup from _tmmGroups.");
            }
        }

        // Best done before the main loop because the loop because it was 1 second since the last iteration, and some
        // games may have started or finished during this time (or timed out). Additionally, players from disbanded
        // matches can be put back into the queue and can immediately find a new match.
        ProcessPendingMatches(now);

        /*
        ++CurrentGeneration;
        Dictionary<int, ProtocolResponse> messagesToSend = new();

        // Sort TMM groups based on a time spent in the queue.
        TMMGroups.Sort(SortByTimeSpentInQueue);

        for (int i = 0; i < TMMGroups.Count; ++i)
        {
            TMMGroup tmmGroup = TMMGroups[i];
            tmmGroup.IsReserved = false;
            tmmGroup.PositionInQueue = i + 1; // starts with #1.
        }

        // Starting with the oldest group, make our way towards the end.
        for (int i = 0; i < TMMGroups.Count;)
        {
            TMMGroup tmmGroup = TMMGroups[i];
            if (tmmGroup.IsReserved)
            {
                ++i;
                continue;
            }
            if (tmmGroup.AccountInfo.Count == 0)
            {
                Logger.Error("Found and removed an empty TMM group.");
                TMMGroups.RemoveAt(i);
                tmmGroup.Status = "Removed from TMMGroups as empty";
                continue;
            }

            TMMGameType gameType = tmmGroup.GameType;
            RankedMatchConfiguration rankedMatchConfiguration = gameType switch
            {
                TMMGameType.MIDWARS => JSONConfiguration.MatchmakingConfiguration.MidWars.Match,
                TMMGameType.CAMPAIGN_CASUAL => JSONConfiguration.MatchmakingConfiguration.Caldavar.Match,
                TMMGameType.CAMPAIGN_NORMAL => JSONConfiguration.MatchmakingConfiguration.Caldavar.Match,
                _ => JSONConfiguration.MatchmakingConfiguration.Caldavar.Match // ignored
            };

            if (tmmGroup.AccountInfo.Count > rankedMatchConfiguration.TeamSize)
            {
                // This group will never be able to find a game...
                EnqueueGroupStatusUpdate(tmmGroup, numMatchingPlayers: -2, 0, "GROUP SIZE TOO BIG!", messagesToSend);

                ++i;
                continue;
            }

            // Select the server to host a game on, either based on preferred region, or by ping.
            List<ConnectedServer> servers;
            if (tmmGroup.SortedPingInformation != null)
            {
                // Pick one server from each of the low-ping managers.
                servers = RecommendServersBasedOnPing(tmmGroup.SortedPingInformation, tmmGroup.PingThreshold - 50);
            }
            else
            {
                // Pick one server from each of the selected regions.
                servers = RecommendServersBasedOnRegions(Random, tmmGroup.Regions);
            }

            int numberOfServers = servers.Count;
            if (numberOfServers == 0)
            {
                // No available servers for any of the selected regions. Try again later.
                EnqueueGroupStatusUpdate(tmmGroup, numMatchingPlayers: -1, 0, string.Join(',', tmmGroup.Regions), messagesToSend);

                ++i;
                continue;
            }

            int bestCompatiblePlayersCount = 0;
            List<TMMGroup> bestCompatibleGroups = null!;
            ConnectedServer bestServer = null!;

            string gameMode = tmmGroup.GameModes.RandomElement();
            long timeInQueueSeconds = (now - tmmGroup.TimestampWhenJoinedQueue) / Stopwatch.Frequency;
            // 0 min = 150, 5 min = 250, 10 min = 350, 15 min = 450
            float balanceThreshold = Math.Min(150.0f + timeInQueueSeconds / 3.0f, 450);
            int teamSize = rankedMatchConfiguration.TeamSize;

            float maximumMmrDifferenceBetweenStrongestAndWeakestPlayer = Math.Max(rankedMatchConfiguration.MaximumPlayerRatingDifference, tmmGroup.HighestMMR - tmmGroup.LowestMMR);
            float lowestCompatibleMMR = tmmGroup.HighestMMR - maximumMmrDifferenceBetweenStrongestAndWeakestPlayer;
            float highestCompatibleMMR = tmmGroup.LowestMMR + maximumMmrDifferenceBetweenStrongestAndWeakestPlayer;
            MatchmakingGroup.TournamentMode tournamentMode = tmmGroup.TournamentMode;

            MatchUp? matchUp = null;
            foreach (ConnectedServer server in servers)
            {
                int managerId = server.ManagerId;
                string region = server.Region;

                List<TMMGroup> compatibleGroups = new();
                int numberOfCompatiblePlayers = tmmGroup.AccountInfo.Count;
                for (int j = i + 1; j < TMMGroups.Count; j++)
                {
                    TMMGroup otherGroup = TMMGroups[j];
                    if (otherGroup.IsReserved)
                    {
                        continue;
                    }
                    if (otherGroup.PingInformation != null)
                    {
                        // Compare by ping.
                        if (otherGroup.PingInformation.TryGetValue(managerId, out int ping))
                        {
                            if (ping > otherGroup.PingThreshold)
                            {
                                // ping too high.
                                continue;
                            }
                        }
                        else
                        {
                            // Unknown ping.
                            continue;
                        }
                    }
                    else
                    {
                        // Compare by region.
                        if (!otherGroup.Regions.Contains(region))
                        {
                            // Incompatible region.
                            continue;
                        }
                    }

                    // See if both teams want to play a Tournament mode, and their team sizes match.
                    if (tournamentMode != MatchmakingGroup.TournamentMode.None && tournamentMode == otherGroup.TournamentMode && tmmGroup.AccountInfo.Count == otherGroup.AccountInfo.Count)
                    {
                        float disparity = Math.Abs(tmmGroup.TournamentMode - otherGroup.TournamentMode);
                        if (disparity < 100)
                        {
                            // Close enough.
                            bestServer = server;
                            matchUp = new MatchUp(new() { tmmGroup }, new() { otherGroup }, disparity);

                            // Note: this is a hack. We need a separate RankedMatchConfiguration for GC.
                            gameMode = "sd";
                            gameType = TMMGameType.GRIMMS_CROSSING_TOURNAMENT;
                            teamSize = 3;
                            break;
                        }
                    }

                    if (!otherGroup.GameModes.Contains(gameMode))
                    {
                        continue;
                    }
                    if (otherGroup.GameType != gameType)
                    {
                        continue;
                    }

                    if (tmmGroup.IsLookingForPlacementMatch && !otherGroup.IsSoloQ)
                    {
                        // Do not match groups with placement solo q.
                        continue;
                    }
                    if (otherGroup.IsLookingForPlacementMatch && !tmmGroup.IsSoloQ)
                    {
                        // Same.
                        continue;
                    }

                    if (otherGroup.HighestMMR > highestCompatibleMMR || otherGroup.LowestMMR < lowestCompatibleMMR)
                    {
                        // MMR disparity too high.
                        continue;
                    }

                    compatibleGroups.Add(otherGroup);
                    numberOfCompatiblePlayers += otherGroup.AccountInfo.Count;
                }

                if (matchUp != null)
                {
                    // Tournament match found.
                    break;
                }

                if (numberOfCompatiblePlayers > bestCompatiblePlayersCount)
                {
                    bestCompatiblePlayersCount = numberOfCompatiblePlayers;
                    bestCompatibleGroups = compatibleGroups;
                    bestServer = server;
                }
            }

            if (matchUp == null)
            {
                EnqueueGroupStatusUpdate(tmmGroup, bestCompatiblePlayersCount, bestServer.ManagerId, bestServer.Name, messagesToSend);

                // See if we have enough players to start a match.
                if (bestCompatiblePlayersCount < rankedMatchConfiguration.TeamSize * 2)
                {
                    // Not enough players to form a match.
                    ++i;

                    // Reserve all compatible players for the remainder of the iteration so that the higher priority group
                    // doesn't have its compatible pool shrink and the game start without them.
                    foreach (TMMGroup group in bestCompatibleGroups)
                    {
                        group.IsReserved = true;
                        EnqueueGroupStatusUpdate(group, bestCompatiblePlayersCount, bestServer.ManagerId, bestServer.Name, messagesToSend);
                    }
                    continue;
                }

                // Check unique permutations, skipping equivalent ones where the teams are
                // identical and only differ in the order of the players, and stopping at the
                // first excellent or better matchup. It's statistically impossible for this
                // method to take long to complete, because if there is a small number of players,
                // forcing us to compute ALL permutations, then the number of such permutations
                // will be low. But if there are larger number of players, then inevitably their
                // MMRs will be fairly close that we will find a matchup with a satisfactory
                // disparity.
                matchUp = UseSmartBruteForce(tmmGroup, bestCompatibleGroups, balanceThreshold, maximumMmrDifferenceBetweenStrongestAndWeakestPlayer, rankedMatchConfiguration, now);
            }

            if (matchUp != null)
            {
                // MatchUp created!
                PendingMatch pendingMatch = new PendingMatch(
                    pendingMatchId: ++NextPendingMatchId,
                    generation: CurrentGeneration,
                    legion: matchUp.Legion,
                    hellbourne: matchUp.Hellbourne,
                    gameMode: gameMode,
                    mapName: GetMapNameByGameType(gameType),
                    gameType: gameType,
                    teamSize: teamSize,
                    pendingServer: bestServer,
                    now: now);
                bestServer.PendingMatch = pendingMatch;

                MoveToPendingMatch(matchUp.Legion, matchUp.Hellbourne, pendingMatch);
                PendingMatches.Add(pendingMatch);

                // Check if server is reachable and then start a match or disband the PendingMatch.
                ServerStatusChecker.Instance.EnqueueRequest(bestServer.Address, bestServer.Port, (isServerPortReachable) => pendingMatch.IsServerPortReachable = isServerPortReachable);

                // Move to the next TMMGroup. Current TMMGroup is removed from the TMMGroups so no need to advance i.
                continue;
            }

            if (false && KongorContext.RuntimeEnvironment != null && bestCompatiblePlayersCount > 20)
            {
                Logger.Error("TMMGroup {0} cannot find a match with {1} compatible players in queue", tmmGroup, bestCompatiblePlayersCount);

                // Log the test case.
                StringBuilder stringBuilder = new();
                stringBuilder.Append(bestCompatiblePlayersCount.ToString());
                stringBuilder.Append(ToDebugString(tmmGroup));

                for (int k = 0; k < bestCompatibleGroups.Count; ++k)
                {
                    stringBuilder.Append(ToDebugString(bestCompatibleGroups[k]));
                }

                File.WriteAllText(String.Format("{0}-{1}.txt", bestCompatiblePlayersCount, Guid.NewGuid()), stringBuilder.ToString());
            }

            // Reserve all compatible players for the remainder of the iteration so that the higher priority group
            // doesn't have its compatible pool shrink and the game start without them.
            //int numGroupsToReserve = 10 - tmmGroup.AccountIds.Length; // No need to reserve too many groups.
            List<TMMGroup> tmmGroupsToReserve;
            //if (bestCompatibleGroups.Count <= numGroupsToReserve)
            //{
            // Reserve all of them.
            tmmGroupsToReserve = bestCompatibleGroups;
            // }
            // else
            // {
            //     // Reserve closest groups by MMR.
            //     float adjustedAverageMMR = tmmGroup.AdjustedAverageMMR;
            //     bestCompatibleGroups.Sort((TMMGroup a, TMMGroup b) => Math.Abs(a.AdjustedAverageMMR - adjustedAverageMMR).CompareTo(Math.Abs(b.AdjustedAverageMMR - adjustedAverageMMR)));
            //     tmmGroupsToReserve = bestCompatibleGroups.GetRange(0, numGroupsToReserve);
            // }

            foreach (TMMGroup group in tmmGroupsToReserve)
            {
                group.IsReserved = true;
                EnqueueGroupStatusUpdate(group, bestCompatiblePlayersCount, bestServer.ManagerId, bestServer.Name, messagesToSend);
            }

            // We were not able to find a match for this group, move on to next.
            ++i;
        }

        lock (typeof(ChatServer.ChatServer))
        {
            // Update everyone in the queue.
            foreach (var pair in messagesToSend)
            {
                if (KongorContext.ConnectedClients.TryGetValue(pair.Key, out var connectedClient))
                {
                    connectedClient.SendResponse(pair.Value);
                }
            }
        }

        MostRecentTMMStats = GetDetailedTMMStats();

        long end = Stopwatch.GetTimestamp();
        if ((end - now) / (float)Stopwatch.Frequency > 0.5f) // don't spam this; print only if it takes > 500ms which indicates there might be a problem
        {
            Console.WriteLine("CurrentGeneration: {0}, time taken (seconds): {1}", CurrentGeneration, (end - now) / (float)Stopwatch.Frequency);
        }
        */
    }
    /*
    private static void EnqueueGroupStatusUpdate(TMMGroup tmmGroup, int numMatchingPlayers, int managerId, string serverName, Dictionary<int, ProtocolResponse> messages)
    {
        for (int i = 0, groupSize = tmmGroup.AccountInfo.Count; i != groupSize; ++i)
        {
            MatchmakingGroupPlayerInfo playerInfo = tmmGroup.AccountInfo[i];
            if (serverName.Length > 15)
            {
                serverName = serverName.Substring(0, 15) + "...";
            }

            string command;
            if (numMatchingPlayers < 0)
            {
                command = string.Format("_tmm_queue_info #{0} All servers are BUSY running other games ({1}) 0", tmmGroup.PositionInQueue, serverName);
            }
            else if (playerInfo.PingInformation.TryGetValue(managerId, out var ping))
            {
                command = string.Format("_tmm_queue_info #{0} {1} players on {2} ping {3} 0", tmmGroup.PositionInQueue, numMatchingPlayers, serverName, ping + 50);
            }
            else
            {
                command = string.Format("_tmm_queue_info #{0} {1} players on {2} 0", tmmGroup.PositionInQueue, numMatchingPlayers, serverName);
            }

            messages[playerInfo.AccountId] = new ChatServer.Client.OptionsResponse(uploadToFTPEnabled: false, uploadToHTTPEnabled: true, false, false, false, true, command);
        }
    }

    internal MatchUp? UseSmartBruteForce(TMMGroup tmmGroup, List<TMMGroup> compatibleGroups, float balanceThreshold, float maximumMmrDifferenceBetweenStrongestAndWeakestPlayer, RankedMatchConfiguration config, long now)
    {
        // Sorting by MMR speeds up the matchmaking significantly (some tests go down from 3 minutes to less that 40ms).
        compatibleGroups.Sort(SortByAdjustedAverageMMR);
        int medianPosition = compatibleGroups.Count / 2;
        bool orderIsInversed = false;
        if (tmmGroup.AdjustedAverageMMR < compatibleGroups[medianPosition].AdjustedAverageMMR)
        {
            orderIsInversed = true;
            compatibleGroups.Reverse();
        }

        List<TMMGroup> teamOne = new();
        List<TMMGroup> teamTwo = new();
        teamOne.Add(tmmGroup);

        return FindBestMatchup(
            teamOneSize: tmmGroup.AccountInfo.Count,
            teamOneMMR: tmmGroup.AdjustedGroupMMR,
            teamOneHighestMMR: tmmGroup.HighestMMR,
            teamOne,
            teamTwoSize: 0,
            teamTwoMMR: 0,
            teamTwoHighestMMR: 0,
            teamTwo,
            lowestMMR: tmmGroup.LowestMMR,
            highestMMR: tmmGroup.HighestMMR,
            compatibleGroups,
            0,
            balanceThreshold,
            orderIsInversed,
            maximumMmrDifferenceBetweenStrongestAndWeakestPlayer,
            config,
            now);
    }

    private MatchUp? FindBestMatchup(
        int teamOneSize,
        float teamOneMMR,
        float teamOneHighestMMR,
        List<TMMGroup> teamOne,
        int teamTwoSize,
        float teamTwoMMR,
        float teamTwoHighestMMR,
        List<TMMGroup> teamTwo,
        float lowestMMR,
        float highestMMR,
        List<TMMGroup> compatiblePool,
        int index,
        float balanceThreshold,
        bool orderIsInversed,
        float maximumMmrDifferenceBetweenStrongestAndWeakestPlayer,
        RankedMatchConfiguration config,
        long now)
    {
        if (teamTwoSize == config.TeamSize)
        {
            // Check if both teams have similar number of groups in them. For example, we only want to match
            // 5q (1 group) vs 4+1, 3+2 (2 groups) or 3+1+1 or 2+2+1 (3 groups).
            if (Math.Abs(teamOne.Count - teamTwo.Count) > 4)
            {
                // Difference is too big, won't make a fair match up.
                return null;
            }

            if (!IsReasonablyBalanced(teamOneMMR, teamTwoMMR, balanceThreshold))
            {
                return null;
            }

            // MatchUp!
            float disparity = Math.Abs(teamOneMMR - teamTwoMMR);
            return new MatchUp(teamOne.ToList(), teamTwo.ToList(), disparity);
        }

        int count = compatiblePool.Count;
        if (index == count)
        {
            return null;
        }

        float lowestMMRGroupInPool;
        float highestMMRGroupInPool;
        if (orderIsInversed)
        {
            highestMMRGroupInPool = compatiblePool[count - 1].AdjustedAverageMMR;
            lowestMMRGroupInPool = compatiblePool[index].AdjustedAverageMMR;
        }
        else
        {
            lowestMMRGroupInPool = compatiblePool[count - 1].AdjustedAverageMMR;
            highestMMRGroupInPool = compatiblePool[index].AdjustedAverageMMR;
        }

        // Check if TeamOne MMR is too high.
        float minimumTeamOneMMR = teamOneMMR + lowestMMRGroupInPool * (5 - teamOneSize);
        float maximumTeamTwoMMR = teamTwoMMR + highestMMRGroupInPool * (5 - teamTwoSize);
        if (minimumTeamOneMMR > maximumTeamTwoMMR && !(IsReasonablyBalanced(minimumTeamOneMMR, maximumTeamTwoMMR, balanceThreshold)))
        {
            // way too high.
            return null;
        }

        // Check if TeamTwo MMR is too high.
        float maximumTeamOneMMR = teamOneMMR + highestMMRGroupInPool * (5 - teamOneSize);
        float minimumTeamTwoMMR = teamTwoMMR + lowestMMRGroupInPool * (5 - teamTwoSize);
        if (minimumTeamTwoMMR > maximumTeamOneMMR && !(IsReasonablyBalanced(minimumTeamTwoMMR, maximumTeamOneMMR, balanceThreshold)))
        {
            // way too low.
            return null;
        }

        MatchUp? bestMatchUp = null;
        for (int i = index; i < compatiblePool.Count; ++i)
        {
            TMMGroup tmmGroup = compatiblePool[i];
            float updatedHighestMMR = Math.Max(highestMMR, tmmGroup.HighestMMR);
            if (orderIsInversed && updatedHighestMMR - lowestMMR > maximumMmrDifferenceBetweenStrongestAndWeakestPlayer)
            {
                // break because all other candidates will be even worse.
                break;
            }

            float updatedLowestMMR = Math.Min(lowestMMR, tmmGroup.LowestMMR);
            if (!orderIsInversed && highestMMR - updatedLowestMMR > maximumMmrDifferenceBetweenStrongestAndWeakestPlayer)
            {
                // break because all other candidates will be even worse.
                break;
            }

            if (updatedHighestMMR - updatedLowestMMR > maximumMmrDifferenceBetweenStrongestAndWeakestPlayer)
            {
                // should we ever get here?
                continue;
            }

            // Add to the smaller team first.
            int tmmGroupSize = tmmGroup.AccountInfo.Count;
            int updatedTeamTwoSize = teamTwoSize + tmmGroupSize;
            if (updatedTeamTwoSize <= config.TeamSize)
            {
                float updatedTeamTwoMMR = teamTwoMMR + tmmGroup.AdjustedGroupMMR;
                float updatedTeamTwoHighestMMR = Math.Max(teamTwoHighestMMR, tmmGroup.HighestMMR);
                if (!ProducesPlusZeroMinusOne(updatedTeamTwoSize, config.TeamSize, updatedTeamTwoMMR, updatedTeamTwoHighestMMR))
                {
                    teamTwo.Add(tmmGroup);
                    MatchUp? matchUp1;
                    if (updatedTeamTwoSize <= teamOneSize)
                    {
                        // regular order.
                        matchUp1 = FindBestMatchup(
                            teamOneSize,
                            teamOneMMR,
                            teamOneHighestMMR,
                            teamOne,
                            updatedTeamTwoSize,
                            updatedTeamTwoMMR,
                            updatedTeamTwoHighestMMR,
                            teamTwo,
                            updatedLowestMMR,
                            updatedHighestMMR,
                            compatiblePool,
                            i + 1,
                            balanceThreshold,
                            orderIsInversed,
                            maximumMmrDifferenceBetweenStrongestAndWeakestPlayer,
                            config,
                            now);
                    }
                    else
                    {
                        // swapped order.
                        matchUp1 = FindBestMatchup(
                            updatedTeamTwoSize,
                            updatedTeamTwoMMR,
                            updatedTeamTwoHighestMMR,
                            teamTwo,
                            teamOneSize,
                            teamOneMMR,
                            teamOneHighestMMR,
                            teamOne,
                            updatedLowestMMR,
                            updatedHighestMMR,
                            compatiblePool,
                            i + 1,
                            balanceThreshold,
                            orderIsInversed,
                            maximumMmrDifferenceBetweenStrongestAndWeakestPlayer,
                            config,
                            now);
                    }

                    if (matchUp1 != null)
                    {
                        if (matchUp1.Disparity < config.ExcellentTeamDisparity)
                        {
                            // Excellent balance, pointless to look further.
                            return matchUp1;
                        }

                        if (bestMatchUp == null || bestMatchUp.Disparity > matchUp1.Disparity)
                        {
                            bestMatchUp = matchUp1;
                        }
                    }

                    teamTwo.RemoveAt(teamTwo.Count - 1);
                }
            }

            int updatedTeamOneSize = teamOneSize + tmmGroupSize;
            if (updatedTeamOneSize <= config.TeamSize)
            {
                float updatedTeamOneMMR = teamOneMMR + tmmGroup.AdjustedGroupMMR;
                float updatedTeamOneHighestMMR = Math.Max(teamOneHighestMMR, tmmGroup.HighestMMR);
                if (!ProducesPlusZeroMinusOne(updatedTeamOneSize, config.TeamSize, updatedTeamOneMMR, updatedTeamOneHighestMMR))
                {
                    teamOne.Add(tmmGroup);

                    MatchUp? matchUp2 = FindBestMatchup(
                        teamOneSize: updatedTeamOneSize,
                        teamOneMMR: updatedTeamOneMMR,
                        teamOneHighestMMR: updatedTeamOneHighestMMR,
                        teamOne,
                        teamTwoSize,
                        teamTwoMMR,
                        teamTwoHighestMMR,
                        teamTwo,
                        updatedLowestMMR,
                        updatedHighestMMR,
                        compatiblePool,
                        i + 1,
                        balanceThreshold,
                        orderIsInversed,
                        maximumMmrDifferenceBetweenStrongestAndWeakestPlayer,
                        config,
                        now);
                    if (matchUp2 != null)
                    {
                        if (matchUp2.Disparity < config.ExcellentTeamDisparity)
                        {
                            // Excellent balance, pointless to look further.
                            return matchUp2;
                        }

                        if (bestMatchUp == null || bestMatchUp.Disparity > matchUp2.Disparity)
                        {
                            bestMatchUp = matchUp2;
                        }
                    }

                    teamOne.RemoveAt(teamOne.Count - 1);
                }
            }
        }

        return bestMatchUp;
    }

    private static bool ProducesPlusZeroMinusOne(int teamSize, int maxTeamSize, float teamMMR, float highestOnTeamMMR)
    {
        if (teamSize != maxTeamSize) return false;

        // A difference of 200 will produce +0/-1.
        // Make the difference at most 169 to allow for everyone to gain at least some MMR.
        float bottomFourMMR = teamMMR - highestOnTeamMMR;
        return (highestOnTeamMMR - (bottomFourMMR / (teamSize - 1))) > 169;
    }

    private MatchUp? UseCaptainsPick(string region, string gameMode, TMMGameType gameType, TMMGroup tmmGroup, List<TMMGroup> compatibleGroups)
    {
        List<TMMGroup> teamOne = new();
        teamOne.Add(tmmGroup);
        int teamOneSize = tmmGroup.AccountInfo.Count;
        float teamOneMMR = tmmGroup.AdjustedGroupMMR;

        List<TMMGroup> teamTwo = new();
        int teamTwoSize = 0;
        float teamTwoMMR = 0;

        while (true)
        {
            if (compatibleGroups.Count == 0)
            {
                return null;
            }

            TMMGroup? groupToAdd = FindBestAddition(5, teamOneSize, teamOneMMR, teamTwoSize, teamTwoMMR, compatibleGroups);
            if (groupToAdd == null)
            {
                // We were not able to find a match up.
                return null;
            }

            // Add best fit to the opposing team.
            compatibleGroups.Remove(groupToAdd);

            teamTwo.Add(groupToAdd);
            teamTwoSize += groupToAdd.AccountInfo.Count;
            teamTwoMMR += groupToAdd.AdjustedGroupMMR;

            // The size of team one cannot be smaller than the size of team two.
            // This is because we are always trying to add players to the smaller team.
            if (teamOneSize < teamTwoSize)
            {
                // Swap them.
                (teamOne, teamTwo) = (teamTwo, teamOne);
                (teamOneSize, teamTwoSize) = (teamTwoSize, teamOneSize);
                (teamOneMMR, teamTwoMMR) = (teamTwoMMR, teamOneMMR);
            }

            if (teamOneSize == 5 && teamTwoSize == 5)
            {
                if (IsReasonablyBalanced(teamOneMMR, teamTwoMMR, 250))
                {
                    return new MatchUp(teamOne, teamTwo, Math.Abs(teamOneMMR - teamTwoMMR));
                }

                // Not balanced enough.
                return null;
            }
        }
    }

    public static void NotifyMatchCreated(PendingMatch pendingMatch, int matchId, string address, short port)
    {
        pendingMatch.ServerInfo = new ServerInfo(matchId, address, port);
    }

    public static void NotifyMatchStartFailed(PendingMatch pendingMatch)
    {
        pendingMatch.ServerInfo = FailedServerInfo;
    }
    */

    private static ProtocolResponse CreateStartMatchResponse(PendingMatch pendingMatch, Random random)
    {
        List<CreateMatchResponse.PlayerInfo> playerInfoList = new();

        List<MatchmakingGroupPlayerInfo> orderedLegion = pendingMatch.Legion.SelectMany(group => group.AccountInfo).OrderBy(info => info.Rating).ToList();

        for (int i = 0; i < pendingMatch.Legion.Count; ++i)
        {
            TMMGroup tmmGroup = pendingMatch.Legion[i];

            playerInfoList.AddRange(tmmGroup.AccountInfo.Select(accountInfo => new CreateMatchResponse.PlayerInfo(AccountId: accountInfo.AccountId, Team: 1, // Legion is 1.
                Slot: Convert.ToByte(orderedLegion.FindIndex(element => element.AccountId == accountInfo.AccountId)),
                GroupSize: Convert.ToByte(tmmGroup.AccountInfo.Count),
                WinMMRDelta: 5.1f, // ignored.
                LossMMRDelta: 6.2f, // ignored.
                Unknown2: 0, GroupIndex: Convert.ToByte(i), Unknown4: 0)));
        }

        List<MatchmakingGroupPlayerInfo> orderedHellbourne = pendingMatch.Hellbourne.SelectMany(group => group.AccountInfo).OrderBy(info => info.Rating).ToList();

        for (int i = 0; i < pendingMatch.Hellbourne.Count; ++i)
        {
            TMMGroup tmmGroup = pendingMatch.Hellbourne[i];

            playerInfoList.AddRange(tmmGroup.AccountInfo.Select(accountInfo => new CreateMatchResponse.PlayerInfo(AccountId: accountInfo.AccountId, Team: 2, // Hellbourne is 2.
                Slot: Convert.ToByte(orderedHellbourne.FindIndex(element => element.AccountId == accountInfo.AccountId)),
                GroupSize: Convert.ToByte(tmmGroup.AccountInfo.Count),
                WinMMRDelta: 5.1f, // ignored.
                LossMMRDelta: 6.2f, // ignored.
                Unknown2: 0, GroupIndex: Convert.ToByte(i), Unknown4: 0)));
        }

        // matchType determines which MMR stats to use (CoN vs MidWars)
        // TODO: introduce an enum?
        byte matchType = pendingMatch.GameType switch
        {
            TMMGameType.MIDWARS => 4,
            TMMGameType.CAMPAIGN_CASUAL => 10,
            TMMGameType.CAMPAIGN_NORMAL => 10,
            TMMGameType.GRIMMS_CROSSING_TOURNAMENT => 7, // Note: 7 refer to RiftWars which we use to piggy-back Tournaments.
            _ => 10 // unknown map
        };

        CreateMatchResponse createMatchResponse = new(
            matchType: matchType,
            matchUp: (int)pendingMatch.GameType,
            unknown1: 0,
            password: random.Next(), // is this used for anything?
            matchNamePrefix: "TMM Match #",
            matchSettings: pendingMatch.MatchSettings,
            useNewMmrSystem: false,
            unknown3: 0, // 1 = heroes already selected? Must be 0.
            playerInfoList: playerInfoList,
            // Currently we don't assign ids to our TMMGroups so the list is blank.
            groupIds: new()
        );

        return createMatchResponse;
    }

    /*
    public static string CreateMatchSettings(string gameMode, string mapName, int teamSize)
    {
        StringBuilder settings = new();
        settings.Append("mode:");
        settings.Append(gameMode);

        if (mapName == "caldavar_old")
        {
            settings.Append(" map:caldavar_old casual:1");
        }
        else
        {
            settings.Append(" map:");
            settings.Append(mapName);
        }

        settings.Append(" teamsize:");
        settings.Append(teamSize.ToString());
        settings.Append(" noleaver:true spectators:10");
        return settings.ToString();
    }

    public static bool IsReasonablyBalanced(float teamOneMMR, float teamTwoMMR, bool strictBalanceConstrains)
    {
        RankedMatchConfiguration match = JSONConfiguration.MatchmakingConfiguration.Caldavar.Match;

        if (strictBalanceConstrains)
        {
            // 284MMR difference is chosen somewhat arbitrarily based on user reports.
            return Math.Abs(teamOneMMR - teamTwoMMR) < match.StrictMaximumTeamDisparity;
        }

        // Increase allowed MMR disparity by 50% after 30 minutes of waiting in queue.
        return Math.Abs(teamOneMMR - teamTwoMMR) < match.RelaxedMaximumTeamDisparity;
    }

    public static bool IsReasonablyBalanced(float teamOneMMR, float teamTwoMMR, float balanceThreshold)
    {
        return Math.Abs(teamOneMMR - teamTwoMMR) < balanceThreshold;
    }
    */

    private void RearmTimer()
    {
        _timer.Change(1000, 1000);
    }

    /*
    /// <summary>
    /// Calculates how badly balanced the 2 teams would be if a potentialMember were added to the team.
    /// </summary>
    private float CalculateFutureTeamDisparity(float teamOneAverageMMR, int teamTwoSize, float teamTwoMMR, TMMGroup potentialMember)
    {
        float teamTwoAverageMMR = (teamTwoMMR + potentialMember.AdjustedGroupMMR) / (teamTwoSize + potentialMember.AccountInfo.Count);
        return Math.Abs(teamOneAverageMMR - teamTwoAverageMMR);
    }

    // From the pool of TMMGroups, pick one group that will best balance the teams without exceeding the team size.
    private TMMGroup? FindBestAddition(int maxTeamSize, int teamOneSize, float teamOneMMR, int teamTwoSize, float teamTwoMMR, List<TMMGroup> pool)
    {
        float leastDisparity = float.MaxValue;
        TMMGroup? bestFit = null;


        float teamOneAverageMMR = teamOneMMR / teamOneSize;
        foreach (TMMGroup tmmGroup in pool)
        {
            int newTeamSize = teamTwoSize + tmmGroup.AccountInfo.Count;
            if (newTeamSize > maxTeamSize)
            {
                // Cannot have more players than max.
                continue;
            }

            float disparity = CalculateFutureTeamDisparity(teamOneAverageMMR, teamTwoSize, teamTwoMMR, tmmGroup);
            if (leastDisparity > disparity)
            {
                // This is an improvement!
                leastDisparity = disparity;
                bestFit = tmmGroup;
            }
        }

        return bestFit;
    }

    internal List<ConnectedServer> RecommendServersBasedOnPing(List<KeyValuePair<int, int>> pingInformation, int pingThreshold)
    {
        List<ConnectedServer>? serversToDisconnect = null;
        List<ConnectedServer> serversToReturn = new();

        for (int i = 0; i < pingInformation.Count; ++i)
        {
            KeyValuePair<int, int> entry = pingInformation[i];
            int ping = entry.Value;
            if (ping > pingThreshold)
            {
                break;
            }

            int potentialManagerId = entry.Key;
            if (BlacklistedManagers.ContainsKey(potentialManagerId))
            {
                // Manager is blacklisted.
                continue;
            }

            // Given a managerId, see if it has any idle servers.
            lock (SharedContext.IdleServersByManagerId)
            {
                if (SharedContext.IdleServersByManagerId.TryGetValue(potentialManagerId, out var servers))
                {
                    foreach (ConnectedServer server in servers)
                    {
                        PendingMatch? pendingMatch = server.PendingMatch;
                        if (pendingMatch != null)
                        {
                            // Optional debug check.
                            if (!PendingMatches.Contains(pendingMatch))
                            {
                                Logger.Error("Game server {0} is marked as having a PendingMatch but the PendingMatch {1} {2} is not registered.", server.Name, pendingMatch.PendingMatchId, pendingMatch.ToString());
                                if (serversToDisconnect == null) serversToDisconnect = new();
                                serversToDisconnect.Add(server);
                            }
                            continue;
                        }

                        // First truly idle server.
                        serversToReturn.Add(server);

                        // Break out of current manager.
                        break;
                    }
                }
            }
        }

        if (serversToDisconnect != null)
        {
            lock (typeof(ChatServer.ChatServer))
            {
                foreach (ConnectedServer serverToDisconnect in serversToDisconnect)
                {
                    serverToDisconnect.Disconnect("Internal bug: Has an unknown PendingMatch.");
                }
            }
        }

        return serversToReturn;
    }

    internal List<ConnectedServer> RecommendServersBasedOnRegions(Random random, HashSet<string> regions)
    {
        List<ConnectedServer>? serversToDisconnect = null;
        List<ConnectedServer> serversToReturn = new();

        List<ConnectedServer> compatibleServers = new();
        lock (SharedContext.IdleServersByRegion)
        {
            foreach (string region in regions)
            {
                if (SharedContext.IdleServersByRegion.TryGetValue(region, out HashSet<ConnectedServer>? servers))
                {
                    foreach (ConnectedServer server in servers)
                    {
                        PendingMatch? pendingMatch = server.PendingMatch;
                        if (pendingMatch != null)
                        {
                            if (!PendingMatches.Contains(pendingMatch))
                            {
                                Logger.Error("Game server {0} is marked as having a PendingMatch but the PendingMatch {1} {2} is not registered.", server.Name, pendingMatch.PendingMatchId, pendingMatch.ToString());
                                if (serversToDisconnect == null) serversToDisconnect = new();
                                serversToDisconnect.Add(server);
                            }
                            continue;
                        }

                        if (BlacklistedManagers.ContainsKey(server.ManagerId))
                        {
                            continue;
                        }

                        compatibleServers.Add(server);

                    }
                }

                int numberOfCompatibleServers = compatibleServers.Count;
                if (numberOfCompatibleServers != 0)
                {
                    // Chose one random server.
                    serversToReturn.Add(compatibleServers[random.Next(0, numberOfCompatibleServers)]);
                    compatibleServers.Clear();
                }
            }
        }

        if (serversToDisconnect != null)
        {
            lock (typeof(ChatServer.ChatServer))
            {
                foreach (ConnectedServer serverToDisconnect in serversToDisconnect)
                {
                    serverToDisconnect.Disconnect("Internal bug: Has an unknown PendingMatch.");
                }
            }
        }

        return serversToReturn;
    }

    private void MoveToPendingMatch(List<TMMGroup> teamOne, List<TMMGroup> teamTwo, PendingMatch pendingMatch)
    {
        int numGroupsRemoved = 0;
        string status = "Moved from TMMGroups to a Pending Match " + pendingMatch.PendingMatchId;

        // Go in reverse order to reduce number of elements moved.
        for (int i = TMMGroups.Count - 1; i >= 0; --i)
        {
            TMMGroup group = TMMGroups[i];
            if (teamOne.Contains(group) || teamTwo.Contains(group))
            {
                TMMGroups.RemoveAt(i);

                group.PendingMatch = pendingMatch;
                group.Status = status;
                ++numGroupsRemoved;
            }
        }

        if (numGroupsRemoved != (teamOne.Count + teamTwo.Count))
        {
            Logger.Error("Could not remove ALL participating groups from the queue, this needs to be investigated!");
        }
    }
    */

    private static void TimerCallback(object? state)
    {
        GameFinder? gameFinder = state as GameFinder;
        if (gameFinder == null) return;

        /*
        PerformanceCounter? performanceCounter = new PerformanceCounter();
        performanceCounter.Category = "GameFinder";
        performanceCounter.Subcategory = "FindGames";

        // Don't fire again until we leave this block.
        gameFinder.Timer.Change(Timeout.Infinite, Timeout.Infinite);

        try
        {
            gameFinder.FindGames(Stopwatch.GetTimestamp());
        }

        catch (Exception e)
        {
            gameFinder.Logger.Error(e, "FindGames");
        }

        gameFinder.RearmTimer();
        performanceCounter.FinishCollection();
        */
    }

    /*
    public string GetTMMStats()
    {
        Dictionary<string, int> numPlayersInQueuePerRegion = new();
        int playersInQueueTotal = 0;

        lock (this)
        {
            foreach (TMMGroup tmmGroup in TMMGroups)
            {
                int numberOfPlayersInGroup = tmmGroup.AccountInfo.Count;
                playersInQueueTotal += numberOfPlayersInGroup;

                foreach (string region in tmmGroup.Regions)
                {
                    int numberOfPlayersQueuedInRegion = 0;
                    numPlayersInQueuePerRegion.TryGetValue(region, out numberOfPlayersQueuedInRegion);
                    numPlayersInQueuePerRegion[region] = numberOfPlayersQueuedInRegion + numberOfPlayersInGroup;
                }
            }
        }

        StringBuilder status = new();
        status.Append("\r\nPlayers in a TMM Queue (total): " + playersInQueueTotal);
        foreach (KeyValuePair<string, int> entry in numPlayersInQueuePerRegion)
        {
            status.Append("\r\nPlayers in a TMM Queue (");
            status.Append(entry.Key);
            status.Append("): ");
            status.Append(entry.Value.ToString());
        }

        return status.ToString();
    }
    */

    private void Disband(PendingMatch pendingMatch, string reason)
    {
        Console.WriteLine("Disbanding pendingMatch {0}, reason: {1}", pendingMatch.PendingMatchId, reason);

        // Disband and add all players back into queue.
        for (int i = pendingMatch.Legion.Count - 1; i >= 0; --i)
        {
            TMMGroup tmmGroup = pendingMatch.Legion[i];
            pendingMatch.Legion.RemoveAt(i);

            if (tmmGroup == null)
            {
                Console.WriteLine("Found null tmmGroup in the Legion team while trying to Disband a PendingMatch");
                continue;
            }
            _tmmGroups.Add(tmmGroup);

            tmmGroup.PendingMatch = null;
        }

        for (int i = pendingMatch.Hellbourne.Count - 1; i >= 0; --i)
        {
            TMMGroup tmmGroup = pendingMatch.Hellbourne[i];
            pendingMatch.Hellbourne.RemoveAt(i);

            if (tmmGroup == null)
            {
                Console.WriteLine("Found null tmmGroup in the Hellbourne team while trying to Disband a PendingMatch");
                continue;
            }
            _tmmGroups.Add(tmmGroup);

            tmmGroup.PendingMatch = null;
        }

        pendingMatch.PendingServer.PendingMatch = null;
    }

    /*
    private string GetDetailedTMMStats()
    {
        // Number of players in queue for region for ALL game types combined (i.e. MidWars, Ranked Normal and Ranked Casual).
        Dictionary<string, int> numPlayersInQueuePerRegion = new();

        // For each game type, keep track of how many players are queued on each region.
        Dictionary<TMMGameType, Dictionary<string, int>> numPlayersInQueuePerRegionByGameType = new();

        int playersInQueueTotal = 0;

        foreach (TMMGroup tmmGroup in TMMGroups)
        {
            TMMGameType gameType = tmmGroup.GameType;

            int numberOfPlayersInGroup = tmmGroup.AccountInfo.Count;
            playersInQueueTotal += numberOfPlayersInGroup;

            Dictionary<string, int> numPlayersInQueuePerRegionForGameType;
            if (numPlayersInQueuePerRegionByGameType.TryGetValue(gameType, out var existingNumPlayersInQueuePerRegion))
            {
                numPlayersInQueuePerRegionForGameType = existingNumPlayersInQueuePerRegion;
            }
            else
            {
                numPlayersInQueuePerRegionForGameType = new Dictionary<string, int>();
                numPlayersInQueuePerRegionByGameType[gameType] = numPlayersInQueuePerRegionForGameType;
            }

            foreach (string region in tmmGroup.Regions)
            {
                int numberOfPlayersQueuedInRegionForGameType = 0;
                numPlayersInQueuePerRegionForGameType.TryGetValue(region, out numberOfPlayersQueuedInRegionForGameType);
                numPlayersInQueuePerRegionForGameType[region] = numberOfPlayersQueuedInRegionForGameType + numberOfPlayersInGroup;

                int numPlayersInQueueForRegion = 0;
                numPlayersInQueuePerRegion.TryGetValue(region, out numPlayersInQueueForRegion);
                numPlayersInQueuePerRegion[region] = numPlayersInQueueForRegion + numberOfPlayersInGroup;
            }
        }

        StringBuilder stringBuilder = new();
        stringBuilder.Append(string.Format("queued:{0}:{1}:{2}|", "ALL", "ALL", playersInQueueTotal));

        foreach (var countByRegion in numPlayersInQueuePerRegion)
        {
            string map = "ALL";
            string region = countByRegion.Key;
            int count = countByRegion.Value;
            stringBuilder.Append(string.Format("queued:{0}:{1}:{2}|", map, region, count));
        }

        foreach (var countByRegionByGameType in numPlayersInQueuePerRegionByGameType)
        {
            TMMGameType gameType = countByRegionByGameType.Key;
            string map = GetMapNameByGameType(gameType);
            foreach (var countByRegion in countByRegionByGameType.Value)
            {
                string region = countByRegion.Key;
                int count = countByRegion.Value;

                stringBuilder.Append(string.Format("queued:{0}:{1}:{2}|", map, region, count));
            }
        }

        return stringBuilder.ToString();
    }

    private static string GetMapNameByGameType(TMMGameType gameType)
    {
        return gameType switch
        {
            TMMGameType.MIDWARS => "midwars",
            TMMGameType.CAMPAIGN_NORMAL => "caldavar",
            TMMGameType.CAMPAIGN_CASUAL => "caldavar_old",
            TMMGameType.GRIMMS_CROSSING_TOURNAMENT => "grimmscrossing",
            _ => "unknown#" + gameType,
        };
    }

    public static string ToDebugString(TMMGroup group)
    {
        StringBuilder message = new StringBuilder("\ngroups.Add(new TMMGroup(new List<MatchmakingGroupPlayerInfo>(){");
        bool first = true;
        foreach (var info in group.AccountInfo)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                message.Append(",");
            }
            message.Append("new(");
            message.Append(info.AccountId);
            message.Append(",");
            string rating = info.Rating.ToString();
            message.Append(rating);
            if (rating.Contains('.')) message.Append("f");
            message.Append(",");
            message.Append(info.IsInPlacements.ToString().ToLower());
            message.Append(")");
        }
        message.Append("},");
        message.Append(group.TimestampWhenJoinedQueue);
        message.Append("));");

        return message.ToString();
    }
    */

    private void ProcessPendingMatches(long now)
    {
        // Process pending matches:
        // 1) PendingMatch that's older than 30 seconds should be disbanded.
        // 2) PendingMatch that has failed to start must be disbanded.
        // 3) PendingMatch that is successful can be removed.

        // This helps keeping PendingMatches removals in one places.
        const int secondsCutoff = 30;
        long cutoffTime = now - Stopwatch.Frequency * secondsCutoff;

        // Iterate backwards to simplify erasing.
        for (int i = _pendingMatches.Count - 1; i >= 0; --i)
        {
            PendingMatch pendingMatch = _pendingMatches[i];
            // ConnectedServer server = pendingMatch.PendingServer;

            ServerInfo? serverInfo = pendingMatch.ServerInfo;
            if (serverInfo == null)
            {
                // Still scheduled.
                if (pendingMatch.CreationTime < cutoffTime)
                {
                    Console.WriteLine("PendingMatch {0} was removed because it was alive for more than {1} seconds.", pendingMatch.PendingMatchId, secondsCutoff);

                    _pendingMatches.RemoveAt(i);
                    Disband(pendingMatch, "Expired");
                    continue;
                }

                // Check if we got a ServerStatusChecker result.
                switch (pendingMatch.IsServerPortReachable)
                {
                    case false:
                        // Server port check complete and came back negative.
                        Console.WriteLine("PendingMatch {0} was disbanded because the server {1} is not reachable", pendingMatch.PendingMatchId, pendingMatch.PendingServer.Name);
                        pendingMatch.PendingServer.Disconnect("Server Is Not Reachable");

                        _pendingMatches.RemoveAt(i);
                        Disband(pendingMatch, "Server Is Not Reachable");
                        break;

                    case true:
                        // Port is reachable, request the server to start a match.
                        ProtocolResponse startMatchResponse = CreateStartMatchResponse(pendingMatch, Random);
                        string? error;
                        lock (typeof(ChatServer.ChatServer))
                        {
                            if (server.Status == ChatServerProtocol.ServerStatus.Idle)
                            {
                                server.PendingMatch = pendingMatch;
                                error = server.SendResponse(startMatchResponse) ? null : "Server: " + server.Name + " with PendingMatch: " + pendingMatch + " is no longer connected";
                            }
                            else
                            {
                                error = "Server is no longer Idle";
                            }
                        }

                        // See if we were able to request a match start.
                        if (error != null)
                        {
                            _pendingMatches.RemoveAt(i);
                            Disband(pendingMatch, error);
                        }

                        // set to null so that we don't do this again.
                        pendingMatch.IsServerPortReachable = null;
                        break;
                }
                continue;
            }

            if (Object.ReferenceEquals(serverInfo, FailedServerInfo))
            {
                // Failed to start.
                _pendingMatches.RemoveAt(i);
                Disband(pendingMatch, "Match failed to start.");
                continue;
            }

            /*
            // Succeeded.
            server.PendingMatch = null;
            server.LastHostedMatchId = serverInfo.MatchId;
            if (pendingMatch.ProtectedMatch)
            {
                server.LastHostedProtectedMatchId = serverInfo.MatchId;
            }

            List<MatchmakingGroupPlayerInfo> legion = pendingMatch.Legion.SelectMany(x => x.AccountInfo).ToList();
            List<MatchmakingGroupPlayerInfo> hellbourne = pendingMatch.Hellbourne.SelectMany(x => x.AccountInfo).ToList();

            // Sort by rating
            legion.Sort((a, b) => a.Rating.CompareTo(b.Rating));
            hellbourne.Sort((a, b) => a.Rating.CompareTo(b.Rating));

            // Initialize MatchInfo for lane calls to work.
            server.MatchInfo = new MatchInfo(
                legionAccountIds: legion.Select(x => x.AccountId).ToArray(),
                legionAccountNames: legion.Select(x => x.AccountName).ToArray(),
                hellbourneAccountIds: hellbourne.Select(x => x.AccountId).ToArray(),
                hellbourneAccountNames: hellbourne.Select(x => x.AccountName).ToArray());

            // Send MatchmakingMatchFoundUpdateResponse which flashes the window to attracts users' attention.
            MatchmakingMatchFoundUpdateResponse matchmakingMatchFoundUpdateResponse = new MatchmakingMatchFoundUpdateResponse(
                pendingMatch.MapName, (byte)pendingMatch.TeamSize, pendingMatch.GameType, pendingMatch.GameMode, region: "", unknown: ""
            );

            AutoMatchConnectBroadcast autoMatchConnectBroadcast = new AutoMatchConnectBroadcast(
                matchType: 4, // ignored, I believe.
                matchId: serverInfo.MatchId,
                address: serverInfo.Address,
                port: serverInfo.Port,
                unknown1: Random.Next()
            );

            MatchmakingGroupQueueUpdateResponse matchmakingGroupQueueUpdateResponse = new MatchmakingGroupQueueUpdateResponse(
                updateType: 16, // Server found, ring horn
                averageTimeInQueueInSeconds: 0 // Ignored.
            );

            pendingMatch.SetStatus("Match created!");
            PendingMatches.RemoveAt(i);
            Console.WriteLine("PendingMatch {0} match created", pendingMatch.PendingMatchId);

            List<MatchmakingGroup> matchmakingGroupsToLeave = new();
            List<int> accountIdsToNotify = new();

            // Remove both teams from the TMM queue.
            foreach (TMMGroup tmmGroup in pendingMatch.BothTeams)
            {
                tmmGroup.PendingMatch = null;
                if (tmmGroup.MatchmakingGroup != null)
                {
                    matchmakingGroupsToLeave.Add(tmmGroup.MatchmakingGroup);
                }

                // Notify match started.
                accountIdsToNotify.AddRange(tmmGroup.AccountInfo.Select(info => info.AccountId));
            }

            pendingMatch.Legion.Clear();
            pendingMatch.Hellbourne.Clear();

            lock (typeof(ChatServer.ChatServer))
            {

                // Remove from the queue.
                foreach (MatchmakingGroup matchmakingGroup in matchmakingGroupsToLeave)
                {
                    matchmakingGroup.NotifyGameServerFound();
                    matchmakingGroup.LeaveQueue(initiatedByGameFinder: true, "Match got created");

                    // SoloQ groups do not persist across different games. In fact, upon
                    // receiving AutoMatchConnectBroadcast event the game will implicitly
                    // consider the group disbanded but without updating the UI.
                    matchmakingGroup.DisbandIfSoloQueueDueToMatchStart();
                }

                // Send match announcement.
                foreach (int accountId in accountIdsToNotify)
                {
                    if (KongorContext.ConnectedClients.TryGetValue(accountId, out var client))
                    {
                        client.SendResponse(matchmakingMatchFoundUpdateResponse);
                        client.SendResponse(matchmakingGroupQueueUpdateResponse);
                        client.SendResponse(autoMatchConnectBroadcast);
                    }
                }
            }
            */
        }
    }
}
