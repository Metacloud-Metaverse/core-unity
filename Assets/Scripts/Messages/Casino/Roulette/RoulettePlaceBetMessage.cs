using Casino;
using Messages.Client;
using Mirror;

namespace Messages.Server
{
	public class RoulettePlaceBetMessage : ClientMessage<RoulettePlaceBetMessage.NetMessage>
	{
		public struct NetMessage : NetworkMessage
		{
			public uint rouletteID;
			public int roulettePositionID;
			public int amount;
		}
		
		public override void Process(NetMessage msg)
		{
			var rouletteObject = LoadNetworkObject(msg.rouletteID);
			var roulette = rouletteObject.GetComponent<Roulette>();
			roulette.ServerPlaceBet(SentByPlayer, msg.amount, msg.roulettePositionID);
		}

		public static void Send(Roulette roulette, RoulettePosition roulettePosition, int newAmount)
		{
			var networkIdentity = roulette.GetComponent<NetworkIdentity>();
			NetMessage msg = new NetMessage()
			{
				rouletteID = networkIdentity.netId,
				roulettePositionID = roulettePosition.ID,
				amount = newAmount
			};
			SendToServer(msg);
		}
	}
}

