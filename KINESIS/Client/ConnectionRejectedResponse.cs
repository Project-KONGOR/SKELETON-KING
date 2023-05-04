namespace KINESIS.Client;

public enum ChatRejectReason
{
    Unknown,
    BadVersion,
    AuthFailed,
    AccountSharing,
    AccountSharingWarning
}

public class ConnectionRejectedResponse : ProtocolResponse
{
    private readonly ChatRejectReason _reason;

    public ConnectionRejectedResponse(ChatRejectReason reason)
    {
        _reason = reason;
    }

    public override CommandBuffer Encode()
    {
        CommandBuffer buffer = new();
        buffer.WriteInt16(ChatServerResponse.ConnectionRejected);
        buffer.WriteInt8(Convert.ToByte(_reason));
        return buffer;
    }
}
