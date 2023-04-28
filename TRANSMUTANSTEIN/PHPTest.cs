namespace TRANSMUTANSTEIN;

[TestClass]
public class PHPTest
{
    [TestMethod]
    public void SerializeBasicObject()
    {
        AuthFailedResponse? authFailedResponse = new(AuthFailureReason.AccountNotFound);
        Assert.AreEqual("a:2:{s:4:\"auth\";s:17:\"Account Not Found\";i:0;b:0;}", PHP.Serialize(authFailedResponse));
    }

    [TestMethod]
    public void SerializeObjectObjectDictionary()
    {
        Dictionary<object, object> dictionary = new()
        {
            ["account_id"] = 12345,
            ["account_name"] = "Maliken",
            [0] = false,
            [long.MinValue] = new Dictionary<object, object> { { "error", new AuthFailedResponse(AuthFailureReason.AccountNotFound) } }
        };

        Assert.AreEqual(
            "a:4:{s:10:\"account_id\";i:12345;s:12:\"account_name\";s:7:\"Maliken\";i:0;b:0;i:-9223372036854775808;a:1:{s:5:\"error\";a:2:{s:4:\"auth\";s:17:\"Account Not Found\";i:0;b:0;}}}",
            PHP.Serialize(dictionary));
    }

    [TestMethod]
    public void SerializeStringObjectDictionary()
    {
        Dictionary<string, object?> dictionary = new()
        {
            ["account_id"] = 12345,
            ["account_name"] = "Maliken",
            ["null"] = null,
            ["0"] = false,
            ["double"] = double.MaxValue,
            ["float"] = float.MaxValue,
            ["long_min"] = long.MinValue,
            ["long_pos"] = 123L,
            ["long_neg"] = -123L,
            ["long_max"] = long.MaxValue
        };
        Assert.AreEqual(
            "a:10:{s:10:\"account_id\";i:12345;s:12:\"account_name\";s:7:\"Maliken\";s:4:\"null\";N;s:1:\"0\";b:0;s:6:\"double\";d:1.7976931348623157E+308;s:5:\"float\";f:3.4028234663852886E+38;s:8:\"long_min\";i:-9223372036854775808;s:8:\"long_pos\";i:123;s:8:\"long_neg\";i:-123;s:8:\"long_max\";i:9223372036854775807;}",
            PHP.Serialize(dictionary));
    }

    [TestMethod]
    public void SerializeStringStringDictionary()
    {
        Dictionary<string, string>? dictionary = new()
        {
            ["account_id"] = "12345",
            ["account_name"] = "Maliken"
        };
        Assert.AreEqual(
            "a:2:{s:10:\"account_id\";s:5:\"12345\";s:12:\"account_name\";s:7:\"Maliken\";}",
            PHP.Serialize(dictionary));
    }

    class Nullable
    {
        public readonly bool? b = null;
        public readonly int? i = null;
        public readonly long? l = null;
        public readonly double? d = null;
        public readonly string? s = null;
        public readonly Dictionary<string, string>? dict = null;
        public readonly object? o = null;
    }

    [TestMethod]
    public void NullableProperties()
    {
        Assert.AreEqual(
            "a:7:{s:1:\"b\";N;s:1:\"i\";N;s:1:\"l\";N;s:1:\"d\";N;s:1:\"s\";N;s:4:\"dict\";N;s:1:\"o\";N;}",
            PHP.Serialize(new Nullable()));
    }

    [TestMethod]
    public void ComplexDictionaryTypes()
    {
        Dictionary<int, long> dictionary = new()
        {
            [123] = 321
        };
        Assert.AreEqual("a:1:{i:123;i:321;}", PHP.Serialize(dictionary));
    }

    [TestMethod]
    public void ListOfStrings()
    {
        List<string> list = new()
        {
            "hello"
        };
        Assert.AreEqual("a:1:{i:0;s:5:\"hello\";}", PHP.Serialize(list));
    }

    [TestMethod]
    public void ListOfInts()
    {
        List<int> list = new()
        {
            123
        };
        Assert.AreEqual("a:1:{i:0;i:123;}", PHP.Serialize(list));
    }
}
