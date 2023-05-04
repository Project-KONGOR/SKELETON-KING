namespace KINESIS;

public class ChatServerResponse
{
    public const short ConnectionAccepted = 0x1C00;
    public const short ConnectionRejected = 0x1C01;

    public const short FullChannelUpdate = 0x0004;
    public const short JoinedChatChannel = 0x0005;
    public const short LeftChatChannel = 0x0006;
    public const short ReceivedChatChannelMessage = 0x0003;
}
