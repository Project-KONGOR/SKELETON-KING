namespace KINESIS.Client;

public class SendChatChannelMessageRequest : ProtocolRequest<ConnectedClient>
{
    private readonly string _message;
    private readonly int _channelId;

    public SendChatChannelMessageRequest(string message, int channelId)
    {
        _message = message;
        _channelId = channelId;
    }

    public static SendChatChannelMessageRequest Decode(byte[] data, int offset, out int updatedOffset)
    {
        SendChatChannelMessageRequest message = new(
            message: ReadString(data, offset, out offset),
            channelId: ReadInt(data, offset, out offset)
        );

        updatedOffset = offset;
        return message;
    }

    public override void HandleRequest(IDbContextFactory<BountyContext> dbContextFactory, ConnectedClient connectedClient)
    {
        if (ChatServer.ChatChannelsByChannelId.TryGetValue(_channelId, out var chatChannel))
        {
            chatChannel.SendMessage(connectedClient.AccountId, _message);
        }
    }
}
