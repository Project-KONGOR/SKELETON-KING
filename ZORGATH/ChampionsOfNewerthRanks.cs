namespace ZORGATH;

public class ChampionsOfNewerthRanks
{
    private static readonly double[] _mmrByRankLookupTable = { 1250, 1275, 1300, 1330, 1365, 1400, 1435, 1470, 1505, 1540, 1575, 1610, 1645, 1685, 1725, 1765, 1805, 1850, 1900, 1950 };
    public static readonly string MmrByRankLookupTableString = string.Join(',', _mmrByRankLookupTable);

    /// <summary>
    ///     Returns the rank for the given MMR value.
    ///     
    ///     NOTE: This function assumes linear mapping between ranks and mmr. In the actual HoN
    ///     server, there was some wiggle room as you'd gain some progress after each rank as a
    ///     cushions so that a single loss wouldn't knock you down a rank.
    /// </summary>
    public static int RankForMmr(double mmr)
    {
        int result = Array.BinarySearch(_mmrByRankLookupTable, mmr);
        if (result >= 0)
        {
            // Element found exactly.
            return result + 1;
        }

        // Not found, which means that it's an inverted index of the next matching rank.
        return ~result;
    }

    /// <summary>
    ///     Returns an integer representing the percent progress until the next rank.
    ///     (E.g. 75 would indicate 75% to the next rank.)
    ///
    ///     NOTE: This function assumes linear mapping between ranks and mmr. In the actual HoN
    ///     server, there was some wiggle room as you'd gain some progress after each rank as a
    ///     cushions so that a single loss wouldn't knock you down a rank.
    /// </summary>
    public static int PercentUntilNextRank(double mmr)
    {
        int rank = RankForMmr(mmr);
        if (rank == (int)ChampionsOfNewerthRank.Immortal)
        {
            // Reached maximum rank.
            return 100;
        }

        double currRankMmr = rank == 0 ? 0 : _mmrByRankLookupTable[rank - 1];
        double nextRankMmr = _mmrByRankLookupTable[rank];
        double percent = (mmr - currRankMmr) / (nextRankMmr - currRankMmr);
        return Convert.ToInt32(percent * 100);
    }
}
