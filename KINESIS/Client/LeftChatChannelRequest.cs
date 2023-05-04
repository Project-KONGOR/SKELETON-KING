namespace ZORGATH.ChatServer.Client;

public class LeftChatChannelRequest : ProtocolRequest<ConnectedClient>
{
    private readonly string _channelName;

    public LeftChatChannelRequest(string channelName)
    {
        _channelName = channelName;
    }

    public static LeftChatChannelRequest Decode(byte[] data, int offset, out int updatedOffset)
    {
        LeftChatChannelRequest message = new(
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
