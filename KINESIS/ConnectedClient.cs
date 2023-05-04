namespace KINESIS;

public class ConnectedClient : IConnectedSubject
{
    private readonly ChatServerConnection<ConnectedClient> _chatServerConnection;
    private int _accountId;

    public ConnectedClient(Socket socket, IProtocolRequestFactory<ConnectedClient> requestFactory, IDbContextFactory<BountyContext> dbContextFactory)
    {
        _chatServerConnection = new(this, socket, requestFactory, dbContextFactory);
    }

    public void Disconnect(string disconnectReason)
    {
        ChatServer.ConnectedClientsByAccountId.Remove(_accountId, out _);
        _chatServerConnection.Stop();
    }

    public void Start()
    {
        _chatServerConnection.Start();
    }

    public void SendResponse(ProtocolResponse response)
    {
        _chatServerConnection.EnqueueResponse(response);
    }

    public void Initialize(int accountId)
    {
        _accountId = accountId;

        // Register new connection, drop old connection if still registered.
        ChatServer.ConnectedClientsByAccountId.AddOrUpdate(accountId, this, (accountId, oldClient) =>
        {
            oldClient.Disconnect("Replaced with a new connection.");
            return this;
        });
    }
}
