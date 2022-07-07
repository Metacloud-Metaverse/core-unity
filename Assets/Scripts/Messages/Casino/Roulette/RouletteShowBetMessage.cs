using Casino;
using Mirror;

namespace Messages.Server
{
    public class RouletteShowBetMessage : ServerMessage<RouletteShowBetMessage.NetMessage>
    {
        public struct NetMessage : NetworkMessage
        {
            
            public uint rouletteID;
        }
		
        public override void Process(NetMessage msg)
        {
            var rouletteObject = LoadNetworkObject(msg.rouletteID);
            var roulette = rouletteObject.GetComponent<Roulette>();

        }

        public static void Send(Roulette roulette, string newText)
        {
            var networkIdentity = roulette.GetComponent<NetworkIdentity>();
            NetMessage msg = new NetMessage()
            {
                rouletteID = networkIdentity.netId,
				
            };
            SendToAllClients(msg);
        }
    }
}