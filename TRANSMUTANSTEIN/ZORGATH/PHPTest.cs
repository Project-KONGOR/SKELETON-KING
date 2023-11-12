using System.Globalization;

namespace ZORGATH;

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
            "a:10:{s:10:\"account_id\";i:12345;s:12:\"account_name\";s:7:\"Maliken\";s:4:\"null\";N;s:1:\"0\";b:0;s:6:\"double\";d:1.7976931348623157E+308;s:5:\"float\";d:3.4028234663852886E+38;s:8:\"long_min\";i:-9223372036854775808;s:8:\"long_pos\";i:123;s:8:\"long_neg\";i:-123;s:8:\"long_max\";i:9223372036854775807;}",
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

    [TestMethod]
    public void NonStandardCulture()
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("is-IS");

        Dictionary<string, object?> dictionary = new()
        {
            ["double"] = double.MaxValue,
            ["float"] = float.MaxValue,
        };
        Assert.AreEqual(
            "a:2:{s:6:\"double\";d:1.7976931348623157E+308;s:5:\"float\";d:3.4028234663852886E+38;}",
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
        Assert.AreEqual("a:0:{}", PHP.Serialize(new Nullable()));
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

    [TestMethod]
    public void SerializeEnum()
    {
        // Enums should serialize as ints
        List<AccountType> listOfEnums = new()
        {
            AccountType.Staff
        };
        List<int> listOfInts = new()
        {
            (int)AccountType.Staff
        };
        Assert.AreEqual(PHP.Serialize(listOfInts), PHP.Serialize(listOfEnums));
    }

    [TestMethod]
    public void SerializeUTF8()
    {
        string utf8 = "coup de grâce";
        Assert.AreEqual("s:14:\"coup de grâce\";", PHP.Serialize(utf8));
    }

    class StaticProperties
    {
        public static int Field;
        public static int Property { get; set; }
    }

    [TestMethod]
    public void StaticPropertiesAreNotSerialized()
    {
        StaticProperties.Field = 1;
        StaticProperties.Property = 2;
        StaticProperties data = new();
        Assert.AreEqual("a:0:{}", PHP.Serialize(data));
    }

    class PrimitiveValues
    {
        public readonly bool b = true;
        public readonly int i = 2;
        public readonly long l = 3;
        public readonly double f = 4.5f;
        public readonly double d = 6.7;
        public readonly string s = "str";
    }
    [TestMethod]
    public void SerializePrimitiveValues()
    {
        Assert.AreEqual("a:6:{s:1:\"b\";b:1;s:1:\"i\";i:2;s:1:\"l\";i:3;s:1:\"f\";d:4.5;s:1:\"d\";d:6.7;s:1:\"s\";s:3:\"str\";}", PHP.Serialize(new PrimitiveValues()));
    }

    [TestMethod]
    public void SerializeStringArray()
    {
        string[] array = new string[] { "1", "2", "3" };
        Assert.AreEqual("a:3:{i:0;s:1:\"1\";i:1;s:1:\"2\";i:2;s:1:\"3\";}", PHP.Serialize(array));
    }

    [TestMethod]
    public void SerializeObjectArray()
    {
        PrimitiveValues?[] array = new PrimitiveValues?[1] { null };
        Assert.AreEqual("a:1:{i:0;N;}", PHP.Serialize(array));
    }

    [TestMethod]
    public void RareCollectionType()
    {
        Dictionary<long, long> d = new()
        {
            [42] = 24
        };
        Assert.AreEqual("a:1:{i:0;i:42;}", PHP.Serialize(d.Keys));
        Assert.AreEqual("a:1:{i:0;i:24;}", PHP.Serialize(d.Values));
    }

    [TestMethod]
    public void SerializeHashSetOfInts()
    {
        HashSet<int> h = new()
        {
            42
        };
        Assert.AreEqual("a:1:{i:0;i:42;}", PHP.Serialize(h));
    }

    [TestMethod]
    public void SerializeHashSetOfStrings()
    {
        HashSet<string> h = new()
        {
            "test"
        };
        Assert.AreEqual("a:1:{i:0;s:4:\"test\";}", PHP.Serialize(h));
    }
}
