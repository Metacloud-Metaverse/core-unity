using Casino;
using Mirror;

namespace Messages.Server
{
	public class RouletteUpdateTextMessage : ServerMessage<RouletteUpdateTextMessage.NetMessage>
	{
		public struct NetMessage : NetworkMessage
		{
			public string text;
			public uint rouletteID;
		}
		
		public override void Process(NetMessage msg)
		{
			var rouletteObject = LoadNetworkObject(msg.rouletteID);
			var roulette = rouletteObject.GetComponent<Roulette>();
			roulette.rouletteInterface.UpdateDataText(msg.text);
		}

		public static void Send(Roulette roulette, string newText)
		{
			var networkIdentity = roulette.GetComponent<NetworkIdentity>();
			NetMessage msg = new NetMessage()
			{
				rouletteID = networkIdentity.netId,
				text = newText
			};
			SendToAllClients(msg);
		}
	}
}