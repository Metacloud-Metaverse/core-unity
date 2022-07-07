using Mirror;
using Casino.Poker;
using UnityEngine;

namespace Messages.Client
{
	public class PokerRequestLeaveJoinMessage : ClientMessage<PokerRequestLeaveJoinMessage.NetMessage>
	{
		public struct NetMessage : NetworkMessage
		{
			public uint pokerID;
			public int playerSeatIndex;
			public bool joining;
		}
		
		public override void Process(NetMessage msg)
		{
			var pokerObject = LoadNetworkObject(msg.pokerID);
			var pokerTable = pokerObject.GetComponent<PokerTable>();
			if (msg.joining)
			{
				pokerTable.TryJoin(msg.playerSeatIndex, SentByPlayer);
			}
			else
			{
				pokerTable.TryLeave(msg.playerSeatIndex, SentByPlayer);
			}
		}

		public static void Send(PokerTable pokerTable, int newPlayerSeatIndex, bool newJoining)
		{
			var networkIdentity = pokerTable.GetComponent<NetworkIdentity>();
			NetMessage msg = new NetMessage()
			{
				pokerID = networkIdentity.netId,
				playerSeatIndex = newPlayerSeatIndex,
				joining = newJoining
			};
			SendToServer(msg);
		}
	}
}