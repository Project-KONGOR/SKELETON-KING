namespace KINESIS;

public interface IProtocolRequestFactory<T>
{
    ProtocolRequest<T>? DecodeProtocolRequest(byte[] buffer, int offset, out int updatedOffset);
}
