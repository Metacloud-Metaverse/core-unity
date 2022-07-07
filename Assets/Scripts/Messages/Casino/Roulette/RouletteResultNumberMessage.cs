using Mirror;
using Casino;

namespace Messages.Server
{
    public class RouletteResultNumberMessage : ServerMessage<RouletteResultNumberMessage.NetMessage>
    {
        public struct NetMessage : NetworkMessage
        {
            public uint rouletteID;
            public int value;
        }
		
        public override void Process(NetMessage msg)
        {
            var rouletteObject = LoadNetworkObject(msg.rouletteID);
            var roulette = rouletteObject.GetComponent<Roulette>();
            roulette.spinner.ApplyRotation(msg.value);
        }

        public static void Send(Roulette roulette, int newValue)
        {
            var networkIdentity = roulette.GetComponent<NetworkIdentity>();
            NetMessage msg = new NetMessage()
            {
                rouletteID = networkIdentity.netId,
                value = newValue
            };
            SendToAllClients(msg);
        }
    }
}