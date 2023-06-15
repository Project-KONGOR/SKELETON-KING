namespace KINESIS.Client;

public class JoinChannelRequest : ProtocolRequest<ConnectedClient>
{
    private readonly string _channelName;

    public JoinChannelRequest(string channelName)
    {
        _channelName = channelName;
    }

    public static JoinChannelRequest Decode(byte[] data, int offset, out int updatedOffset)
    {
        JoinChannelRequest message = new(
            channelName: ReadString(data, offset, out offset)
        );

        updatedOffset = offset;
        return message;
    }

    public override void HandleRequest(IDbContextFactory<BountyContext> dbContextFactory, ConnectedClient connectedClient)
    {
        if (_channelName == "KONGOR")
        {
            // Special case.
            int i = 1;
            string channelName = string.Format("KONGOR {0}", i);
            while (!TryJoiningChannel(channelName, channelName.ToUpper(), connectedClient))
            {
                ++i;
            }
        }
        else
        {
            TryJoiningChannel(_channelName, _channelName.ToUpper(), connectedClient);
        }
    }

    private static bool TryJoiningChannel(string channelName, string upperCaseChannelName, ConnectedClient connectedClient)
    {
        ChatChannel chatChannel;
        if (ChatServer.ChatChannelsByUpperCaseName.TryGetValue(upperCaseChannelName, out var existingChatChannel))
        {
            // Existing channel is found.
            if (existingChatChannel.Flags.HasFlag(ChatChannelFlags.CannotBeJoined))
            {
                // Cannot be joined manually.
                return false;
            }

            if (upperCaseChannelName.StartsWith("CLAN "))
            {
                // Do not let people not in the clan join.
                string clanNameUpperCase = upperCaseChannelName[5..]; // Skip the "Clan " part of the "Clan <Clan Name>".
                if (clanNameUpperCase != connectedClient.ClientInformation.UpperCaseClanName)
                {
                    // Player doesn't belong to this clan.
                    return false;
                }
            }

            chatChannel = existingChatChannel;
        }
        else
        {
            // Existing channel is NOT found.
            ChatChannelFlags flagsToCreateChatChannelWith;
            if (upperCaseChannelName.StartsWith("CLAN "))
            {
                // Do not let people not in the clan join.
                string clanNameUpperCase = upperCaseChannelName[5..]; // Skip the "Clan " part of the "Clan <Clan Name>".
                if (clanNameUpperCase != connectedClient.ClientInformation.UpperCaseClanName)
                {
                    // Player doesn't belong to this clan.
                    return false;
                }

                // Unclear if Reserved is necessary or not.
                flagsToCreateChatChannelWith = ChatChannelFlags.Reserved | ChatChannelFlags.Clan;
            }
            else
            {
                flagsToCreateChatChannelWith = ChatChannelFlags.GeneralUse;
            }

            chatChannel = new ChatChannel(channelName, upperCaseChannelName, $"Welcome To The {channelName} Channel", flagsToCreateChatChannelWith);
            if (!ChatServer.ChatChannelsByUpperCaseName.TryAdd(upperCaseChannelName, chatChannel))
            {
                // Chat channel already exists? Try again.
                return TryJoiningChannel(channelName, upperCaseChannelName, connectedClient);
            }
        }

        return chatChannel.Add(connectedClient);
    }
}
