namespace KINESIS;

public abstract class ProtocolResponse
{
    private CommandBuffer? _cachedInstance;

    public abstract CommandBuffer Encode();

    public CommandBuffer CommandBuffer
    {
        get
        {
            _cachedInstance ??= Encode();
            return _cachedInstance;
        }
    }
}
