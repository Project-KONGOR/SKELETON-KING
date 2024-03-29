﻿namespace KINESIS.Server;

public class ConnectServerRequest : ProtocolRequest<ConnectedServer>
{
    private readonly int _serverId;
    private readonly string _sessionCookie;
#pragma warning disable IDE0052
    private readonly int _chatProtocolVersion;
#pragma warning restore IDE0052

    public ConnectServerRequest(int serverId, string sessionCookie, int chatProtocolVersion)
    {
        _serverId = serverId;
        _sessionCookie = sessionCookie;
        _chatProtocolVersion = chatProtocolVersion;
    }

    public static ConnectServerRequest Decode(byte[] data, int offset, out int updatedOffset)
    {
        ConnectServerRequest message = new ConnectServerRequest(
            serverId: ReadInt(data, offset, out offset),
            sessionCookie: ReadString(data, offset, out offset),
            chatProtocolVersion: ReadInt(data, offset, out offset)
        );

        updatedOffset = offset;
        return message;
    }

    public override void HandleRequest(IDbContextFactory<BountyContext> dbContextFactory, ConnectedServer connectedServer)
    {
        using var bountyContext = dbContextFactory.CreateDbContext();
        int accountId = bountyContext.GameServers
            .Where(gameServer => gameServer.GameServerId == _serverId && gameServer.Cookie == _sessionCookie && (gameServer.Account.AccountType == AccountType.Staff || gameServer.Account.AccountType == AccountType.RankedMatchHost || gameServer.Account.AccountType == AccountType.UnrankedMatchHost))
            .Select(gameServer => gameServer.AccountId)
            .FirstOrDefault();
        if (accountId == 0)
        {
            connectedServer.SendResponse(new ServerConnectionRejectedResponse(ConnectionRejectedReason.AuthFailed));
            connectedServer.Disconnect("Failed to authenticate the game server.");
            return;
        }

        ChatServer.ConnectedServersByServerId.AddOrUpdate(
            _serverId,
            connectedServer,
            (serverId, previousConnectedServer) => {
                previousConnectedServer.SendResponse(new ServerConnectionRejectedResponse(ConnectionRejectedReason.AccountSharing));
                previousConnectedServer.Disconnect("The server was replaced with a new instance.");
                return connectedServer;
            }
        );

        connectedServer.Initialize(accountId);
        connectedServer.SendResponse(new ServerConnectionAcceptedResponse());
    }
}
