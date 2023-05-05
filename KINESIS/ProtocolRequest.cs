namespace KINESIS;

public abstract class ProtocolRequest<T>
{
    public abstract void HandleRequest(IDbContextFactory<BountyContext> dbContextFactory, T subject);
}
