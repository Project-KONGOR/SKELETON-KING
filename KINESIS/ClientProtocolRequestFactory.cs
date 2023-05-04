namespace KINESIS;

public class ClientProtocolRequestFactory : IProtocolRequestFactory<ConnectedClient>
{
    public ProtocolRequest<ConnectedClient>? DecodeProtocolRequest(byte[] buffer, int offset, out int updatedOffset)
    {
        updatedOffset = offset;

        int messageId = BitConverter.ToInt16(buffer, offset);

        // Advance offset by 2 bytes that we just read. Note that we don't want to advance updatedOffset
        // unless the message is recognized.
        offset += 2;

        return messageId switch
        {
            ChatServerRequest.Connect => Client.ConnectRequest.Decode(buffer, offset, out updatedOffset),
            ChatServerRequest.JoinChatChannel => Client.JoinChannelRequest.Decode(buffer, offset, out updatedOffset),
            ChatServerRequest.LeaveChatChannel => Client.LeaveChatChannelRequest.Decode(buffer, offset, out updatedOffset),
            ChatServerRequest.SendChatChannelMessage => Client.SendChatChannelMessageRequest.Decode(buffer, offset, out updatedOffset),

            // Unknown message.
            _ => null,
        };
    }
}
