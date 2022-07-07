using Casino;
using Messages.Client;
using Mirror;

public class RequestCasinoChipsMessage : ClientMessage<RequestCasinoChipsMessage.NetMessage>
{
    public struct NetMessage : NetworkMessage {}
		
    public override void Process(NetMessage msg)
    {
        CasinoManager.Instance.ServerRequestFunds(SentByPlayer);
    }

    public static void Send()
    {
        NetMessage msg = new NetMessage();
        SendToServer(msg);
    }
}
