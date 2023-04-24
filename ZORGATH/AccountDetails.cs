namespace ZORGATH;

// A small subset of account information containing (mostly) player stats.
public class AccountDetails
{
    private static readonly int _numPlacementMatchesForRank = 6;

    public readonly int AccountId;

    public readonly float PublicRating;
    public readonly int PublicGamesPlayed;
    public readonly int PublicTimesDisconnected;

    public readonly float MidWarsRating;
    public readonly int MidWarsGamesPlayed;
    public readonly int MidWarsTimesDisconnected;
    public readonly int MidWarsRank;

    public readonly float CoNNormalRating;
    public readonly int CoNNormalGamesPlayed;
    public readonly int CoNNormalTimesDisconnected;
    public readonly int CoNNormalRank;

    public readonly float CoNCasualRating;
    public readonly int CoNCasualGamesPlayed;
    public readonly int CoNCasualTimesDisconnected;
    public readonly int CoNCasualRank;

    public readonly int TournamentGamesPlayed;
    public readonly int TournamentTimesDisconnected;

    public AccountDetails(
        int accountId,
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
        int tournamentTimesDisconnected)
    {
        AccountId = accountId;

        PublicRating = publicRating;
        PublicGamesPlayed = publicGamesWon + publicGamesLost;
        PublicTimesDisconnected = publicTimesDisconnected;

        MidWarsRating = midWarsRating;
        MidWarsGamesPlayed = midWarsGamesWon + midWarsGamesLost;
        MidWarsTimesDisconnected = midWarsTimesDisconnected;
        MidWarsRank = midWarsPlacementsDone < _numPlacementMatchesForRank ? 0 : ChampionsOfNewerthRanks.RankForMmr(midWarsRating);

        CoNNormalRating = conNormalRating;
        CoNNormalGamesPlayed = conNormalGamesWon + conNormalGamesLost;
        CoNNormalTimesDisconnected = conNormalTimesDisconnected;
        CoNNormalRank = conNormalPlacementsDone < _numPlacementMatchesForRank ? 0 : ChampionsOfNewerthRanks.RankForMmr(conNormalRating);

        CoNCasualRating = conCasualRating;
        CoNCasualGamesPlayed = conCasualGamesWon + conCasualGamesLost;
        CoNCasualTimesDisconnected = conCasualTimesDisconnected;
        CoNCasualRank = conCasualPlacementsDone < _numPlacementMatchesForRank ? 0 : ChampionsOfNewerthRanks.RankForMmr(conCasualRating);

        TournamentGamesPlayed = tournamentGamesWon + tournamentGamesLost;
        TournamentTimesDisconnected = tournamentTimesDisconnected;
    }
}
