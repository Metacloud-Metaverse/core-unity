using Casino;
using Mirror;
using Casino.Poker;

namespace Messages.Server
{
	public class PokerPlayerCardMessage : ServerMessage<PokerPlayerCardMessage.NetMessage>
	{
		public struct NetMessage : NetworkMessage
		{
			public uint pokerID;
			public PokerCard card;
			public int cardIndex;
			public int playerIndex;
		}
		
		public override void Process(NetMessage msg)
		{
			var pokerObject = LoadNetworkObject(msg.pokerID);
			var pokerTable = pokerObject.GetComponent<PokerTable>();
			pokerTable.pokerInterface.AssignPlayerCard(msg.cardIndex, msg.playerIndex, msg.card);
		}

		public static void SendToAll(PokerTable pokerTable, PokerCard newCard, int newCardIndex, int newPlayerIndex, bool hidden, ConnectedPlayer connectedPlayer)
		{
			var networkIdentity = pokerTable.GetComponent<NetworkIdentity>();
			NetMessage msg = new NetMessage()
			{
				pokerID = networkIdentity.netId,
				card = newCard,
				cardIndex = newCardIndex,
				playerIndex = newPlayerIndex
			};
			SendToClient(connectedPlayer.networkConnection, msg);

			if (hidden)
			{
				msg = new NetMessage()
				{
					pokerID = networkIdentity.netId,
					card = CasinoManager.Instance.upsideDownCard,
					cardIndex = newCardIndex,
					playerIndex = newPlayerIndex
				};
			}
			SendToAllClientsExcept(connectedPlayer.networkConnection, msg);
			
		}

		public static void Send(PokerTable pokerTable, int newCardIndex, int newPlayerIndex, ConnectedPlayer connectedPlayer)
		{
			var networkIdentity = pokerTable.GetComponent<NetworkIdentity>();
			NetMessage msg = new NetMessage()
			{
				pokerID = networkIdentity.netId,
				card = CasinoManager.Instance.upsideDownCard,
				cardIndex = newCardIndex,
				playerIndex = newPlayerIndex
			};
			SendToClient(connectedPlayer.networkConnection, msg);
		}
	}
}