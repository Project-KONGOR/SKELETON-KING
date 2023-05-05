namespace ZORGATH;

[TestClass]
public class AccountStatsTest
{
    [TestMethod]
    public void TestAccountStatsConstruction()
    {
        AccountStats accountStats = new AccountStats(
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
               allOtherGameDisconnects: 7);
        Assert.AreEqual(1, accountStats.Level);
        Assert.AreEqual(2.5f, accountStats.LevelExp);
        Assert.AreEqual(1500, accountStats.PSR);
        Assert.AreEqual(1650.2f, accountStats.NormalRankedGamesMMR);
        Assert.AreEqual(1534.1f, accountStats.CasualModeMMR);
        Assert.AreEqual(12, accountStats.PublicGamesPlayed);
        Assert.AreEqual(13, accountStats.NormalRankedGamesPlayed);
        Assert.AreEqual(14, accountStats.CasualModeGamesPlayed);
        Assert.AreEqual(15, accountStats.MidWarsGamesPlayed);
        Assert.AreEqual(16, accountStats.AllOtherGamesPlayed);
        Assert.AreEqual(3, accountStats.PublicGameDisconnects);
        Assert.AreEqual(4, accountStats.NormalRankedGameDisconnects);
        Assert.AreEqual(5, accountStats.CasualModeDisconnects);
        Assert.AreEqual(6, accountStats.MidWarsTimesDisconnected);
        Assert.AreEqual(7, accountStats.AllOtherGameDisconnects);
    }
}
