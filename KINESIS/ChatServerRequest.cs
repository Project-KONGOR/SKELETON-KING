namespace KINESIS;

public class ChatServerRequest
{
    public const short Connect = 0x0C00;
    public const short JoinChatChannel = 0x001E;
    public const short LeaveChatChannel = 0x0022;
    public const short SendChatChannelMessage = 0x0003;
}
