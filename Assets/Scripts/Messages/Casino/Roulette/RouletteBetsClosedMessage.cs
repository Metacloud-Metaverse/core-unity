using Mirror;
using Casino;

namespace Messages.Server
{
	public class RouletteBetsClosedMessage : ServerMessage<RouletteBetsClosedMessage.NetMessage>
	{
		public struct NetMessage : NetworkMessage
		{
			public uint rouletteID;
			public bool value;
		}
		
		public override void Process(NetMessage msg)
		{
			var rouletteObject = LoadNetworkObject(msg.rouletteID);
			var roulette = rouletteObject.GetComponent<Roulette>();
			roulette.rouletteInterface.ToggleBets(msg.value);
		}

		public static void Send(Roulette roulette, bool newValue)
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