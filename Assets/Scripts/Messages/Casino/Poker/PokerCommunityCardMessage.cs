using Casino;
using Mirror;
using Casino.Poker;

namespace Messages.Server
{
	public class PokerCommunityCardMessage : ServerMessage<PokerCommunityCardMessage.NetMessage>
	{
		public struct NetMessage : NetworkMessage
		{
			public uint pokerID;
			public PokerCard card;
			public int cardIndex;
		}
		
		public override void Process(NetMessage msg)
		{
			var pokerObject = LoadNetworkObject(msg.pokerID);
			var pokerTable = pokerObject.GetComponent<PokerTable>();
			pokerTable.pokerInterface.AssignCommunityCard(msg.cardIndex, msg.card);
		}

		public static void Send(PokerTable pokerTable, PokerCard newCard, int newCardIndex, ConnectedPlayer connectedPlayer = null)
		{
			var networkIdentity = pokerTable.GetComponent<NetworkIdentity>();
			NetMessage msg = new NetMessage()
			{
				pokerID = networkIdentity.netId,
				card = newCard,
				cardIndex = newCardIndex
			};
			if (connectedPlayer != null)
			{
				
			}
			else
			{
				SendToAllClients(msg);
			}
		}
	}
}