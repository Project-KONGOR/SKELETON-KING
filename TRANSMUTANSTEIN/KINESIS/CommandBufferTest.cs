using KINESIS;

namespace TRANSMUTANSTEIN.KINESIS;

[TestClass]
public class CommandBufferTest
{
    [TestMethod]
    public void TestCommandBuffer()
    {
        CommandBuffer commandBuffer = new CommandBuffer();
        commandBuffer.WriteInt8(1);  // offset = 0 -> 1
        commandBuffer.WriteInt16(2); // offset = 1 -> 3
        commandBuffer.WriteInt32(3); // offset = 3 -> 7
        commandBuffer.WriteInt64(4); // offset = 7 -> 15
        commandBuffer.WriteFloat32(5.0f); // offset = 15 -> 19
        commandBuffer.WriteString("hello world"); // offset = 19 -> 31

        commandBuffer.WriteString("hello world"); // offset = 31 -> 42
        commandBuffer.WriteFloat32(6.0f); // offset = 42 -> 46
        commandBuffer.WriteInt64(7); // offset = 46 -> 54
        commandBuffer.WriteInt32(8); // offset = 54 -> 58
        commandBuffer.WriteInt16(9); // offset = 59 -> 61
        commandBuffer.WriteInt8(10);  // offset = 61 -> 62

        commandBuffer.WriteInt32(42);  // offset = 62 -> 66

        byte[] data = commandBuffer.Buffer;
        int size = commandBuffer.Size;
        Assert.AreEqual(66, commandBuffer.Size);

        int offset = 0;
        Assert.AreEqual(1, ProtocolRequest<int>.ReadByte(data, offset, out offset));
        Assert.AreEqual(2, ProtocolRequest<int>.ReadShort(data, offset, out offset));
        Assert.AreEqual(3, ProtocolRequest<int>.ReadInt(data, offset, out offset));
        Assert.AreEqual(4, ProtocolRequest<int>.ReadLong(data, offset, out offset));
        Assert.AreEqual(5.0f, ProtocolRequest<int>.ReadFloat(data, offset, out offset));
        Assert.AreEqual("hello world", ProtocolRequest<int>.ReadString(data, offset, out offset));
        Assert.AreEqual("hello world", ProtocolRequest<int>.ReadString(data, offset, out offset));
        Assert.AreEqual(6.0f, ProtocolRequest<int>.ReadFloat(data, offset, out offset));
        Assert.AreEqual(7, ProtocolRequest<int>.ReadLong(data, offset, out offset));
        Assert.AreEqual(8, ProtocolRequest<int>.ReadInt(data, offset, out offset));
        Assert.AreEqual(9, ProtocolRequest<int>.ReadShort(data, offset, out offset));
        Assert.AreEqual(10, ProtocolRequest<int>.ReadByte(data, offset, out offset));
        Assert.AreEqual(42, ProtocolRequest<int>.ReadInt(data, offset, out offset));
        Assert.AreEqual(size, offset);
    }
}
