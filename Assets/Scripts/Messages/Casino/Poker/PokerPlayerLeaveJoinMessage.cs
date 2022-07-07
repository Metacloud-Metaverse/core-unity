using Mirror;
using Casino.Poker;

namespace Messages.Server
{
	public class PokerPlayerLeaveJoinMessage : ServerMessage<PokerPlayerLeaveJoinMessage.NetMessage>
	{
		public struct NetMessage : NetworkMessage
		{
			public uint pokerID;
			public int playerSeatIndex;
			public PokerSeatOccupiedStatus occupiedStatus;
		}
		
		public override void Process(NetMessage msg)
		{
			var pokerObject = LoadNetworkObject(msg.pokerID);
			var pokerTable = pokerObject.GetComponent<PokerTable>();
			pokerTable.pokerInterface.AssignPlayerSeat(msg.playerSeatIndex, msg.occupiedStatus);
		}

		public static void SendToAll(PokerTable pokerTable, int newPlayerSeatIndex, bool newJoining, ConnectedPlayer connectedPlayer = null)
		{
			var networkIdentity = pokerTable.GetComponent<NetworkIdentity>();
			NetMessage msg = new NetMessage()
			{
				pokerID = networkIdentity.netId,
				playerSeatIndex = newPlayerSeatIndex,
				occupiedStatus = newJoining ? PokerSeatOccupiedStatus.occupied : PokerSeatOccupiedStatus.empty
			};
			if (newJoining)
			{
				SendToAllClientsExcept(connectedPlayer.networkConnection, msg);
				
				msg = new NetMessage()
				{
					pokerID = networkIdentity.netId,
					playerSeatIndex = newPlayerSeatIndex,
					occupiedStatus = PokerSeatOccupiedStatus.owned
				};
				
				SendToClient(connectedPlayer.networkConnection, msg);
			}
			else
			{
				SendToAllClients(msg);
			}
		}

		public static void Send(PokerTable pokerTable, int newPlayerSeatIndex, bool isOccupied, ConnectedPlayer connectedPlayer)
		{
			var networkIdentity = pokerTable.GetComponent<NetworkIdentity>();
			NetMessage msg = new NetMessage()
			{
				pokerID = networkIdentity.netId,
				playerSeatIndex = newPlayerSeatIndex,
				occupiedStatus = isOccupied ? PokerSeatOccupiedStatus.occupied : PokerSeatOccupiedStatus.empty
			};
			SendToClient(connectedPlayer.networkConnection, msg);
		}
	}
}

//TODO: replace with some client id once clients know about other clients
public enum PokerSeatOccupiedStatus
{
	empty,
	occupied,
	owned
}