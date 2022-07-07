using Mirror;
using Casino.Poker;

namespace Messages.Server
{
	public class PokerChipsUpdate : ServerMessage<PokerChipsUpdate.NetMessage>
	{
		public struct NetMessage : NetworkMessage
		{
			public uint pokerID;
			public int value;
			public bool isBetPool;
			public int playerIndex;
			public int availableCash;
		}

		public override void Process(NetMessage msg)
		{
			var pokerObject = LoadNetworkObject(msg.pokerID);
			var pokerTable = pokerObject.GetComponent<PokerTable>();
			if (msg.isBetPool)
			{
				pokerTable.pokerInterface.AssignBetPoolChips(msg.value);
			}
			else
			{
				pokerTable.pokerInterface.AssignPlayerChips(msg.value, msg.playerIndex, msg.availableCash);
			}
		}

		public static void Send(PokerTable pokerTable, int newValue, bool newIsBetPool, int newPlayerIndex = 0, int newAvailableCash = 0, ConnectedPlayer connectedPlayer = null)
		{
			var networkIdentity = pokerTable.GetComponent<NetworkIdentity>();
			NetMessage msg = new NetMessage()
			{
				pokerID = networkIdentity.netId,
				value = newValue,
				isBetPool = newIsBetPool,
				playerIndex = newPlayerIndex,
				availableCash = newAvailableCash
			};
			if (connectedPlayer == null)
			{
				SendToAllClients(msg);
			}
			else
			{
				SendToClient(connectedPlayer.networkConnection, msg);
			}
		}
	}
}