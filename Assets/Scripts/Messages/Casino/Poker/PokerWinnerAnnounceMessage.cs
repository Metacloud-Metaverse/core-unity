using Casino;
using Mirror;
using Casino.Poker;

namespace Messages.Server
{
	public class PokerWinnerAnnounceMessage : ServerMessage<PokerWinnerAnnounceMessage.NetMessage>
	{
		public struct NetMessage : NetworkMessage
		{
			public uint pokerID;
			public string value;
		}
		
		public override void Process(NetMessage msg)
		{
			var pokerObject = LoadNetworkObject(msg.pokerID);
			var pokerTable = pokerObject.GetComponent<PokerTable>();
			pokerTable.pokerInterface.AnnounceWinner(msg.value);
		}

		public static void Send(PokerTable pokerTable, string newValue, ConnectedPlayer connectedPlayer = null)
		{
			var networkIdentity = pokerTable.GetComponent<NetworkIdentity>();
			NetMessage msg = new NetMessage()
			{
				pokerID = networkIdentity.netId,
				value = newValue
			};
			if (connectedPlayer != null)
			{
				SendToClient(connectedPlayer.networkConnection, msg);
			}
			else
			{
				SendToAllClients(msg);
			}
		}
	}
}