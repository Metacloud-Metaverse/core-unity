using Messages.Client;
using Messages.Server;
using Mirror;
using Network;
using UnityEngine;

public class RequestWorldTeleportMessage : ClientMessage<RequestWorldTeleportMessage.NetMessage>
{
    public struct NetMessage : NetworkMessage
    {
        public Vector3 value;
    }
		
    public override void Process(NetMessage msg)
    {
        TeleportToVectorMessage.Send(SentByPlayer.networkConnection.identity, msg.value);
    }

    public static void Send(Vector3 newValue)
    {
        NetMessage msg = new NetMessage()
        {
            value = newValue,
        };
        SendToServer(msg);
    }
}