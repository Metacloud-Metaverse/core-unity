using System.Collections.Generic;
using Mirror;
using Casino;

namespace Messages.Server
{
	public class RouletteLastWinningNumbersMessage : ServerMessage<RouletteLastWinningNumbersMessage.NetMessage>
	{
		public struct NetMessage : NetworkMessage
		{
			public uint rouletteID;
			public List<int> winningNumbers;
		}
		
		public override void Process(NetMessage msg)
		{
			var rouletteObject = LoadNetworkObject(msg.rouletteID);
			var roulette = rouletteObject.GetComponent<Roulette>();
			roulette.rouletteInterface.DisplayLastWinningNumbers(msg.winningNumbers);
		}

		public static void Send(Roulette roulette, List<int> lastWinningNumbers)
		{
			var networkIdentity = roulette.GetComponent<NetworkIdentity>();
			NetMessage msg = new NetMessage()
			{
				rouletteID = networkIdentity.netId,
				winningNumbers = lastWinningNumbers
			};
			SendToAllClients(msg);
		}
	}
}