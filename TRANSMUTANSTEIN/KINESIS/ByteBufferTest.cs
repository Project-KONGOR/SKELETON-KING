namespace KINESIS;

[TestClass]
public class ByteBufferTest
{
    [TestMethod]
    public void EnsureCapacityShiftsDataNewBuffer()
    {
        ByteBuffer byteBuffer = new();

        // Write 256 bytes of data
        int bytesWritten = 256;
        for (int i = 0; i < bytesWritten; ++i)
        {
            byteBuffer.Buffer[i] = Convert.ToByte(i);
        }
        byteBuffer.WriteOffset = bytesWritten;

        // Read 100 bytes of data.
        int bytesRead = 100;
        byteBuffer.ReadOffset = bytesRead;

        // Request increase in buffer size.
        byteBuffer.EnsureCapacity(byteBuffer.Buffer.Length + 1);
        Assert.AreEqual(0, byteBuffer.ReadOffset);
        Assert.AreEqual(bytesWritten - bytesRead, byteBuffer.WriteOffset);

        for (int i = 0; i < 256 - bytesRead; ++i)
        {
            Assert.AreEqual(Convert.ToByte(bytesRead + i), byteBuffer.Buffer[i]);
        }
    }

    [TestMethod]
    public void EnsureCapacityShiftsDataInPlace()
    {
        ByteBuffer byteBuffer = new();

        // Write 256 bytes of data
        int bytesWritten = 256;
        for (int i = 0; i < bytesWritten; ++i)
        {
            byteBuffer.Buffer[i] = Convert.ToByte(i);
        }
        byteBuffer.WriteOffset = bytesWritten;

        // Read 100 bytes of data.
        int bytesRead = 100;
        byteBuffer.ReadOffset = bytesRead;

        // Request increase in buffer size. This should shift the data in-place instead of allocating a new buffer.
        byteBuffer.EnsureCapacity(byteBuffer.Buffer.Length - bytesRead / 2);
        Assert.AreEqual(0, byteBuffer.ReadOffset);
        Assert.AreEqual(bytesWritten - bytesRead, byteBuffer.WriteOffset);

        for (int i = 0; i < 256 - bytesRead; ++i)
        {
            Assert.AreEqual(Convert.ToByte(bytesRead + i), byteBuffer.Buffer[i]);
        }
    }

    [TestMethod]
    public void EnsureCapacityIsNoopIfSpaceAvailable()
    {
        ByteBuffer byteBuffer = new();

        // Write 256 bytes of data
        int bytesWritten = 256;
        for (int i = 0; i < bytesWritten; ++i)
        {
            byteBuffer.Buffer[i] = Convert.ToByte(i);
        }
        byteBuffer.WriteOffset = bytesWritten;

        // Read 100 bytes of data.
        int bytesRead = 100;
        byteBuffer.ReadOffset = bytesRead;

        // Request capacity that is already available.
        byteBuffer.EnsureCapacity(byteBuffer.Buffer.Length - bytesWritten);
        Assert.AreEqual(bytesRead, byteBuffer.ReadOffset);
        Assert.AreEqual(bytesWritten, byteBuffer.WriteOffset);

        for (int i = 0; i < bytesWritten; ++i)
        {
            Assert.AreEqual(Convert.ToByte(i), byteBuffer.Buffer[i]);
        }
    }
}
