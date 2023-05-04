namespace KINESIS.Client;

public class ReceivedChatChannelMessageResponse : ProtocolResponse
{
    private readonly int _accountId;
    private readonly int _channelId;
    private readonly string _message;

    public ReceivedChatChannelMessageResponse(int accountId, int channelId, string message)
    {
        _accountId = accountId;
        _channelId = channelId;
        _message = message;
    }

    public override CommandBuffer Encode()
    {
        CommandBuffer buffer = new();
        buffer.WriteInt16(ChatServerResponse.ReceivedChatChannelMessage);
        buffer.WriteInt32(_accountId);
        buffer.WriteInt32(_channelId);
        buffer.WriteString(_message);
        return buffer;
    }
}
