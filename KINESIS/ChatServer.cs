namespace KINESIS;

public class ChatServer
{
    private readonly IDbContextFactory<BountyContext> _dbContextFactory;
    private readonly IProtocolRequestFactory<ConnectedClient> _clientProtocolRequestFactory;
    private readonly TcpListener _clientListener;

    public static readonly ConcurrentDictionary<int, ConnectedClient> ConnectedClientsByAccountId = new();

    public ChatServer(IDbContextFactory<BountyContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;

        // TODO: make ports configurable.
        _clientListener = new TcpListener(IPAddress.Any, 11031);
        _clientProtocolRequestFactory = new ClientProtocolRequestFactory();
    }

    public void Start()
    {
        _clientListener.Start();
        _clientListener.BeginAcceptSocket(AcceptClientSocketCallback, this);
    }

    public void Stop()
    {
        _clientListener.Stop();
    }

    public static void AcceptClientSocketCallback(IAsyncResult result)
    {
        ChatServer chatServer = (result.AsyncState as ChatServer)!;

        TcpListener clientListener = chatServer._clientListener;
        Socket socket;
        try
        {
            socket = clientListener.EndAcceptSocket(result);

            ConnectedClient client = new(socket, chatServer._clientProtocolRequestFactory, chatServer._dbContextFactory);
            client.Start();
        }
        finally
        {
            // Prepare for another connection.
            clientListener.BeginAcceptSocket(AcceptClientSocketCallback, chatServer);
        }
    }
}
