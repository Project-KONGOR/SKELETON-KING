namespace KINESIS.Client;

public class LeaveChatChannelRequest : ProtocolRequest<ConnectedClient>
{
    private readonly string _channelName;

    public LeaveChatChannelRequest(string channelName)
    {
        _channelName = channelName;
    }

    public static LeaveChatChannelRequest Decode(byte[] data, int offset, out int updatedOffset)
    {
        LeaveChatChannelRequest message = new(
            channelName: ReadString(data, offset, out offset)
        );

        updatedOffset = offset;
        return message;
    }

    public override void HandleRequest(IDbContextFactory<BountyContext> dbContextFactory, ConnectedClient connectedClient)
    {
        if (ChatServer.ChatChannelsByUpperCaseName.TryGetValue(_channelName.ToUpper(), out var chatChannel))
        {
            chatChannel.Remove(connectedClient, notifyClient: false);

            // No need to send a response to the client as the client already closed the channel.
            connectedClient.NotifyRemovedFromChatChannel(chatChannel, response: null);
        }
    }
}
