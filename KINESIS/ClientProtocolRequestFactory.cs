namespace KINESIS;

public class ClientProtocolRequestFactory : IProtocolRequestFactory<ConnectedClient>
{
    public ProtocolRequest<ConnectedClient>? DecodeProtocolRequest(byte[] buffer, int offset, out int updatedOffset)
    {
        updatedOffset = offset;

        int messageId = BitConverter.ToInt16(buffer, offset);
        return messageId switch
        {
            // Unknown message.
            _ => null,
        };
    }
}