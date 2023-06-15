namespace KINESIS.Client;

public class LeftChatChannelResponse : ProtocolResponse
{
    private readonly int _accountId;
    private readonly int _channelId;

    public LeftChatChannelResponse(int accountId, int channelId)
    {
        _accountId = accountId;
        _channelId = channelId;
    }

    public override CommandBuffer Encode()
    {
        CommandBuffer buffer = new();
        buffer.WriteInt16(ChatServerResponse.LeftChatChannel);
        buffer.WriteInt32(_accountId);
        buffer.WriteInt32(_channelId);
        return buffer;
    }
}
