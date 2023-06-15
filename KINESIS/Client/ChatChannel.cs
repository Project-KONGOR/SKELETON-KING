namespace KINESIS.Client;

public class ChatChannelUser
{
    public readonly string DisplayedName;
    public readonly int AccountId;
    public ChatClientStatus ChatClientStatus;
    public readonly ChatAdminLevel ChatAdminLevel;
    public readonly string ChatSymbol;
    public readonly string ChatNameColour;
    public readonly string AccountIcon;
    public readonly int AscensionLevel;

    public ChatChannelUser(string displayedName, int accountId, ChatClientStatus chatClientStatus, ChatAdminLevel chatAdminLevel, string chatSymbol, string chatNameColour, string accountIcon, int ascensionLevel)
    {
        DisplayedName = displayedName;
        AccountId = accountId;
        ChatClientStatus = chatClientStatus;
        ChatAdminLevel = chatAdminLevel;
        ChatSymbol = chatSymbol;
        ChatNameColour = chatNameColour;
        AccountIcon = accountIcon;
        AscensionLevel = ascensionLevel;
    }
}

public class ChatChannel
{
    private static int _lastChannelId = 0;
    private readonly int _id;
    private readonly string _name;
    private readonly string _topic;
    private readonly ChatChannelFlags _flags;
    private readonly List<ChatChannelUser> _users = new();
    private bool _channelIsFull = false;

    public ChatChannelFlags Flags => _flags;

    public ChatChannel(string name, string upperCaseName, string topic, ChatChannelFlags flags)
    {
        _id = Interlocked.Increment(ref _lastChannelId);
        _name = name;
        _topic = topic;
        _flags = flags;

        ChatServer.ChatChannelsByChannelId[_id] = this;
        ChatServer.ChatChannelsByUpperCaseName[upperCaseName] = this;
    }

    public bool Add(ConnectedClient connectedClient)
    {
        if (_channelIsFull)
        {
            // Channel exceeds maximum allowed size.
            return false;
        }

        int accountId = connectedClient.AccountId;
        ClientInformation clientInformation = connectedClient.ClientInformation;
        ChatChannelUser newUser = new(
            clientInformation.DisplayedName,
            accountId,
            clientInformation.ChatClientStatus,
            ChatAdminLevel.None, // TODO: implement
            clientInformation.SelectedChatSymbolCode,
            clientInformation.SelectedChatNameColourCode,
            clientInformation.SelectedAccountIconCode,
            clientInformation.AscensionLevel);

        FullChannelUpdateResponse fullChannelUpdateResponse;
        lock (this)
        {
            if (_channelIsFull)
            {
                // Cannot accept any more users without risking packet size overflow.
                return false;
            }

            if (_users.Any(user => user.AccountId == accountId))
            {
                // Already in the channel.
                return false;
            }

            fullChannelUpdateResponse = new(
                channelName: _name,
                channelId: _id,
                chatChannelFlags: _flags,
                channelTopic: _topic,
                channelUsers: _users
            );

            // Channel responses can be fairly big and can overflow the packet size limit. We don't want that since the client will reject it anyway.
            // Mark the channel as full until another player leaves the channel.
            _channelIsFull = fullChannelUpdateResponse.CommandBuffer.Size > ProtocolResponse.MAXIMUM_RESPONSE_SIZE;
            if (_channelIsFull)
            {
                // Cannot add another user to this channel.
                return false;
            }

            _users.Add(newUser);
        }

        // Notify old users about the new account before adding it the the list
        // to avoid sending to ourselves.
        JoinedChannelResponse joinedChannelResponse = new(_id, newUser);
        foreach (var existingUser in fullChannelUpdateResponse.ChannelUsers)
        {
            if (ChatServer.ConnectedClientsByAccountId.TryGetValue(existingUser.AccountId, out var existingUserClient))
            {
                existingUserClient.SendResponse(joinedChannelResponse);
            }
        }

        // Notify the new user now.
        connectedClient.NotifyAddedToChatChannel(this, fullChannelUpdateResponse);
        return true;
    }

    public void Remove(ConnectedClient client, bool notifyClient)
    {
        ChatChannelUser[] remainingUsers;
        lock (this)
        {
            if (_users.RemoveAll(p => p.AccountId == client.AccountId) == 0)
            {
                Console.WriteLine("ChatChannel.Remove did not find anyone to remove.");
                return;
            }

            _channelIsFull = false;
            if (_users.Count == 0)
            {
                // Channel became empty. Dispose of it.
                ChatServer.ChatChannelsByUpperCaseName.TryRemove(_name.ToUpper(), out _);
                ChatServer.ChatChannelsByChannelId.TryRemove(_id, out _);

                remainingUsers = Array.Empty<ChatChannelUser>();
            }
            else
            {
                remainingUsers = _users.ToArray();
            }
        }

        LeftChatChannelResponse response = new(accountId: client.AccountId, channelId: _id);
        foreach (ChatChannelUser remainingUser in remainingUsers)
        {
            if (ChatServer.ConnectedClientsByAccountId.TryGetValue(remainingUser.AccountId, out var remainingUserClient))
            {
                remainingUserClient.SendResponse(response);
            }
        }

        if (notifyClient)
        {
            client.NotifyRemovedFromChatChannel(this, response);
        }
    }
}
