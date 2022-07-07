using Mirror;
using Casino.Poker;

namespace Messages.Server
{
	public class PokerPlayerTurnUpdateMessage : ServerMessage<PokerPlayerTurnUpdateMessage.NetMessage>
	{
		public struct NetMessage : NetworkMessage
		{
			public uint pokerID;
			public int nextPlayerIndex;
			public PokerDecision decision;
			public int lastPlayerIndex;
		}
		
		public override void Process(NetMessage msg)
		{
			var pokerObject = LoadNetworkObject(msg.pokerID);
			var pokerTable = pokerObject.GetComponent<PokerTable>();
			pokerTable.pokerInterface.ChangeTurn(msg.nextPlayerIndex);
		}

		public static void Send(PokerTable pokerTable, int newNextPlayerIndex, PokerDecision newDecision = PokerDecision.Call, int newLastPlayerIndex = 0)
		{
			var networkIdentity = pokerTable.GetComponent<NetworkIdentity>();
			NetMessage msg = new NetMessage()
			{
				pokerID = networkIdentity.netId,
				nextPlayerIndex = newNextPlayerIndex,
				decision = newDecision,
				lastPlayerIndex = newLastPlayerIndex
			};
			SendToAllClients(msg);
		}
	}
}