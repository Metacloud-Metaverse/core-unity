using Mirror;
using Casino.Poker;

namespace Messages.Server
{
	public class PokerGamePhaseMessage : ServerMessage<PokerGamePhaseMessage.NetMessage>
	{
		public struct NetMessage : NetworkMessage
		{
			public uint pokerID;
			public PokerPhase pokerPhase;
		}
		
		public override void Process(NetMessage msg)
		{
			var pokerObject = LoadNetworkObject(msg.pokerID);
			var pokerTable = pokerObject.GetComponent<PokerTable>();
			if (msg.pokerPhase == PokerPhase.GameEnd)
			{
				pokerTable.pokerInterface.HandEnded();
			}
		}

		public static void Send(PokerTable pokerTable, PokerPhase newPokerPhase, ConnectedPlayer connectedPlayer = null)
		{
			var networkIdentity = pokerTable.GetComponent<NetworkIdentity>();
			NetMessage msg = new NetMessage()
			{
				pokerID = networkIdentity.netId,
				pokerPhase = newPokerPhase
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