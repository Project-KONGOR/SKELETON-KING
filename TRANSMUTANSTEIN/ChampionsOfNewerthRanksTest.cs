using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TRANSMUTANSTEIN;

[TestClass]
public class ChampionsOfNewerthRanksTest
{
    [DataTestMethod]
    [DataRow(1000, ChampionsOfNewerthRank.NoRank)]
    [DataRow(1100, ChampionsOfNewerthRank.NoRank)]
    [DataRow(1200, ChampionsOfNewerthRank.NoRank)]
    [DataRow(1220, ChampionsOfNewerthRank.NoRank)]
    [DataRow(1237, ChampionsOfNewerthRank.NoRank)]
    [DataRow(1238, ChampionsOfNewerthRank.NoRank)]
    [DataRow(1250, ChampionsOfNewerthRank.BronzeFive)]
    [DataRow(1275, ChampionsOfNewerthRank.BronzeFour)]
    [DataRow(1276, ChampionsOfNewerthRank.BronzeFour)]
    [DataRow(1300, ChampionsOfNewerthRank.BronzeThree)]
    [DataRow(1313, ChampionsOfNewerthRank.BronzeThree)]
    [DataRow(1314, ChampionsOfNewerthRank.BronzeThree)]
    [DataRow(1350, ChampionsOfNewerthRank.BronzeTwo)]
    [DataRow(1351, ChampionsOfNewerthRank.BronzeTwo)]
    [DataRow(1352, ChampionsOfNewerthRank.BronzeTwo)]
    [DataRow(1375, ChampionsOfNewerthRank.BronzeOne)]
    [DataRow(1389, ChampionsOfNewerthRank.BronzeOne)]
    [DataRow(1390, ChampionsOfNewerthRank.BronzeOne)]
    [DataRow(1420, ChampionsOfNewerthRank.SilverFive)]
    [DataRow(1427, ChampionsOfNewerthRank.SilverFive)]
    [DataRow(1428, ChampionsOfNewerthRank.SilverFive)]
    [DataRow(1450, ChampionsOfNewerthRank.SilverFour)]
    [DataRow(1465, ChampionsOfNewerthRank.SilverFour)]
    [DataRow(1466, ChampionsOfNewerthRank.SilverFour)]
    [DataRow(1500, ChampionsOfNewerthRank.SilverThree)]
    [DataRow(1503, ChampionsOfNewerthRank.SilverThree)]
    [DataRow(1504, ChampionsOfNewerthRank.SilverThree)]
    [DataRow(1530, ChampionsOfNewerthRank.SilverTwo)]
    [DataRow(1541, ChampionsOfNewerthRank.SilverOne)]
    [DataRow(1542, ChampionsOfNewerthRank.SilverOne)]
    [DataRow(1550, ChampionsOfNewerthRank.SilverOne)]
    [DataRow(1579, ChampionsOfNewerthRank.GoldFour)]
    [DataRow(1580, ChampionsOfNewerthRank.GoldFour)]
    [DataRow(1600, ChampionsOfNewerthRank.GoldFour)]
    [DataRow(1617, ChampionsOfNewerthRank.GoldThree)]
    [DataRow(1618, ChampionsOfNewerthRank.GoldThree)]
    [DataRow(1640, ChampionsOfNewerthRank.GoldThree)]
    [DataRow(1655, ChampionsOfNewerthRank.GoldTwo)]
    [DataRow(1656, ChampionsOfNewerthRank.GoldTwo)]
    [DataRow(1675, ChampionsOfNewerthRank.GoldTwo)]
    [DataRow(1693, ChampionsOfNewerthRank.GoldOne)]
    [DataRow(1694, ChampionsOfNewerthRank.GoldOne)]
    [DataRow(1720, ChampionsOfNewerthRank.GoldOne)]
    [DataRow(1731, ChampionsOfNewerthRank.DiamondThree)]
    [DataRow(1732, ChampionsOfNewerthRank.DiamondThree)]
    [DataRow(1750, ChampionsOfNewerthRank.DiamondThree)]
    [DataRow(1769, ChampionsOfNewerthRank.DiamondTwo)]
    [DataRow(1770, ChampionsOfNewerthRank.DiamondTwo)]
    [DataRow(1800, ChampionsOfNewerthRank.DiamondTwo)]
    [DataRow(1807, ChampionsOfNewerthRank.DiamondOne)]
    [DataRow(1808, ChampionsOfNewerthRank.DiamondOne)]
    [DataRow(1820, ChampionsOfNewerthRank.DiamondOne)]
    [DataRow(1845, ChampionsOfNewerthRank.DiamondOne)]
    [DataRow(1846, ChampionsOfNewerthRank.DiamondOne)]
    [DataRow(1875, ChampionsOfNewerthRank.LegendaryTwo)]
    [DataRow(1883, ChampionsOfNewerthRank.LegendaryTwo)]
    [DataRow(1884, ChampionsOfNewerthRank.LegendaryTwo)]
    [DataRow(1900, ChampionsOfNewerthRank.LegendaryOne)]
    [DataRow(1921, ChampionsOfNewerthRank.LegendaryOne)]
    [DataRow(1922, ChampionsOfNewerthRank.LegendaryOne)]
    [DataRow(1950, ChampionsOfNewerthRank.Immortal)]
    [DataRow(2000, ChampionsOfNewerthRank.Immortal)]
    public void TestRankForMmr(double mmr, int expectedRank)
    {
        int rank = ChampionsOfNewerthRanks.RankForMmr(mmr);
        Assert.AreEqual(expectedRank, rank);
    }

    [DataTestMethod]
    [DataRow(1100, 88)] // Precomputed.
    [DataRow(1250, 0)]
    [DataRow(1315, 50)]  // Exactly halfway between a rank
    [DataRow(1280, 20)]  // Precomputed. Rank bounds [1275-1300]
    [DataRow(1900, 0)]
    [DataRow(1939, 78)]  // Precomputed.
    [DataRow(1949, 98)]  // Precomputed.
    [DataRow(2000, 100)]
    [DataRow(2050, 100)]
    public void TestPercentUntilNextRank(double mmr, int expectedPercent)
    {
        int percent = ChampionsOfNewerthRanks.PercentUntilNextRank(mmr);
        Assert.AreEqual(expectedPercent, percent);
    }
}
