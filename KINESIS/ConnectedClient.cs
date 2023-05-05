namespace KINESIS;

public class ConnectedClient : IConnectedSubject
{
    private readonly ChatServerConnection<ConnectedClient> _chatServerConnection;

    public ConnectedClient(Socket socket, IProtocolRequestFactory<ConnectedClient> requestFactory, IDbContextFactory<BountyContext> dbContextFactory)
    {
        _chatServerConnection = new(this, socket, requestFactory, dbContextFactory);
    }

    public void Disconnect(string disconnectReason)
    {
        _chatServerConnection.Stop();
    }

    public void Start()
    {
        _chatServerConnection.Start();
    }
}
