namespace KINESIS;

public class ChatServerConnection<T> where T : IConnectedSubject
{
    private readonly ByteBuffer _receiveBuffer = new();
    private readonly ByteBuffer _sendBuffer = new();
    private readonly T _subject;
    private readonly Socket _socket;
    private readonly IProtocolRequestFactory<T> _requestFactory;
    private readonly IDbContextFactory<BountyContext> _dbContextFactory;
    private bool _socketClosed = false;

    public ChatServerConnection(T subject, Socket socket, IProtocolRequestFactory<T> requestFactory, IDbContextFactory<BountyContext> dbContextFactory)
    {
        _subject = subject;
        _socket = socket;
        _requestFactory = requestFactory;
        _dbContextFactory = dbContextFactory;
    }

    public void Start()
    {
        ResumeReceivingFromSocket();
    }

    public void Stop()
    {
        lock (_socket)
        {
            if (!_socketClosed)
            {
                _socket.Close();
                _socketClosed = true;
            }
        }
    }

    private void ResumeReceivingFromSocket()
    {
        // Make sure we have some space to read a message.
        _receiveBuffer.EnsureCapacity(512);
        byte[] buffer = _receiveBuffer.Buffer;
        int writeOffset = _receiveBuffer.WriteOffset;

        IAsyncResult result;
        lock (_socket)
        {
            if (_socketClosed)
            {
                return;
            }

            result = _socket.BeginReceive(buffer, writeOffset, buffer.Length - writeOffset, SocketFlags.None, OnDataReceived, this);
        }

        if (result.CompletedSynchronously)
        {
            OnDataReceivedImpl(result);
        }
    }

    private static void OnDataReceived(IAsyncResult ar)
    {
        if (ar.CompletedSynchronously)
        {
            // Return to avoid holding the socket lock for the duration of message processing.
            // Otherwise, this may (and will) cause a deadlock if we try to send a message
            // to the socket.
            return;
        }

        ChatServerConnection<T> connection = (ar.AsyncState as ChatServerConnection<T>)!;
        connection.OnDataReceivedImpl(ar);
    }

    private void OnDataReceivedImpl(IAsyncResult ar)
    {
        Socket socket = _socket;
        int numberOfBytesReceived;
        lock (socket)
        {
            if (_socketClosed)
            {
                // Socket is no longer operational.
                return;
            }

            numberOfBytesReceived = socket.EndReceive(ar, out SocketError errorCode);
        }

        if (numberOfBytesReceived == 0)
        {
            // Connection has been lost. Close the socket to avoid sending any more data.
            lock (typeof(ChatServer))
            {
                _subject.Disconnect("Connection Has Been Closed");
            }
            return;
        }

        ByteBuffer receiveBuffer = _receiveBuffer;
        byte[] buffer = receiveBuffer.Buffer;

        // Save old value so that we can validate that they are still the same when we update them.
        int expectedReceiveBufferWriteOffset = _receiveBuffer.WriteOffset;
        int expectedReceiveBufferReadOffset = _receiveBuffer.ReadOffset;

        int originalReadOffset = receiveBuffer.ReadOffset;
        int originalWriteOffset = receiveBuffer.WriteOffset;

        int messageEnd = originalReadOffset;
        if (messageEnd > originalWriteOffset)
        {
            throw new Exception(string.Format("ReadOffset {0} > WriteOffset {1} from the beginning.", messageEnd, originalWriteOffset));
        }

        int writeOffset = originalWriteOffset + numberOfBytesReceived;
        if (messageEnd > writeOffset)
        {
            throw new Exception(string.Format("ReadOffset {0} > WriteOffset {1} after receiving data, numberOfBytesReceived: {2}.", messageEnd, writeOffset, numberOfBytesReceived));
        }

        int readOffset;
        while (true)
        {
            // Always start at the end of the last message.
            readOffset = messageEnd;

            if (readOffset > writeOffset)
            {
                throw new Exception(string.Format("ReadOffset {0} > WriteOffset {1} before processing a message. ByteBuffer ReadOffset: {2} WriteOffset: {3} Length: {4}", readOffset, writeOffset, originalReadOffset, writeOffset, buffer.Length));
            }

            // See if we read everything there is to read.
            if (readOffset == writeOffset)
            {
                // Since we read everything, we can reset the offsets and have more space in the buffer to read/write.
                readOffset = 0;
                writeOffset = 0;
                break;
            }

            int numberOfBytesAvailableToRead = (writeOffset - readOffset);

            const int messageHeaderSize = 4; // 2 bytes for command size and 2 bytes of command code.
            if (numberOfBytesAvailableToRead < messageHeaderSize)
            {
                // Not enough data to even read the message header.
                break;
            }

            int messageLength = BitConverter.ToInt16(buffer, readOffset);
            if (numberOfBytesAvailableToRead < messageLength + 2)
            {
                // Not enough data to decode the entire message. Wait for more data and try again.
                break;
            }

            int messageStart = readOffset + 2;
            messageEnd = messageStart + messageLength;
            if (messageEnd > buffer.Length)
            {
                throw new Exception(string.Format("messageEnd {0} > buffer.Length {1} messageLength {2} numberOfBytesAvailableToRead: {3}", messageEnd, buffer.Length, messageLength, numberOfBytesAvailableToRead));
            }

            ProtocolRequest<T>? message = _requestFactory.DecodeProtocolRequest(buffer, messageStart, out int updatedOffset);
            if (message == null)
            {
                // Unknown message. Just skip it for now.
                Console.WriteLine("Unknown message has been received: 0x{0:X4}", BitConverter.ToInt16(buffer, messageStart));
                continue;
            }

            if (updatedOffset != messageEnd)
            {
                throw new Exception(string.Format("Incorrect number of bytes read for message: {0}", BitConverter.ToInt16(buffer, messageStart)));
            }

            message.HandleRequest(_dbContextFactory, _subject);
        }

        if (_receiveBuffer.WriteOffset != expectedReceiveBufferWriteOffset)
        {
            throw new Exception(string.Format("ReceiveBuffer.WriteOffset {0} != expectedReceiveBufferWriteOffset {1}", _receiveBuffer.WriteOffset, expectedReceiveBufferWriteOffset));
        }

        if (_receiveBuffer.ReadOffset != expectedReceiveBufferReadOffset)
        {
            throw new Exception(string.Format("ReceiveBuffer.ReadOffset {0} != expectedReceiveBufferReadOffset {1}", _receiveBuffer.ReadOffset, expectedReceiveBufferReadOffset));
        }

        if (readOffset > writeOffset)
        {
            throw new Exception(string.Format("ReadOffset {0} > WriteOffset {1}", readOffset, writeOffset));
        }

        receiveBuffer.ReadOffset = readOffset;
        receiveBuffer.WriteOffset = writeOffset;

        ResumeReceivingFromSocket();
    }

    private static void OnDataSent(IAsyncResult ar)
    {
        if (ar.CompletedSynchronously)
        {
            // Return to avoid holding the socket lock for the duration of message processing.
            // Otherwise, this may (and will) cause a deadlock if we try to send a message
            // to the socket.
            return;
        }

        ChatServerConnection<T> connection = (ar.AsyncState as ChatServerConnection<T>)!;
        connection.OnDataSentImpl(ar);
    }

    private bool OnDataSentImpl(IAsyncResult ar)
    {
        Socket socket = _socket;
        int numberOfBytesSent;
        lock (socket)
        {
            if (_socketClosed)
            {
                // Socket is no longer operational.
                return false;
            }

            numberOfBytesSent = socket.EndSend(ar, out SocketError errorCode);
        }

        if (numberOfBytesSent == 0)
        {
            // Connection has been lost. Close the socket to avoid sending any more data.
            lock (typeof(ChatServer))
            {
                _subject.Disconnect("Connection Has Been Closed");
            }
            return false;
        }

        ByteBuffer sendBuffer = _sendBuffer;
        lock (sendBuffer)
        {
            int readOffset = sendBuffer.ReadOffset;
            int writeOffset = sendBuffer.WriteOffset;
            readOffset += numberOfBytesSent;

            if (readOffset == writeOffset)
            {
                // Sent overything we have.
                sendBuffer.ReadOffset = 0;
                sendBuffer.WriteOffset = 0;

                return true;
            }
            else
            {
                // We have more data to send.
                sendBuffer.ReadOffset = readOffset;

                IAsyncResult result;
                lock (socket)
                {
                    // Only try and send if socket is still operational.
                    if (_socketClosed)
                    {
                        return false;
                    }

                    result = socket.BeginSend(sendBuffer.Buffer, readOffset, writeOffset - readOffset, SocketFlags.None, OnDataSent, this);
                }

                if (result.CompletedSynchronously)
                {
                    return OnDataSentImpl(result);
                }
                else
                {
                    // Did not fail.
                    return true;
                }
            }
        }
    }
}
