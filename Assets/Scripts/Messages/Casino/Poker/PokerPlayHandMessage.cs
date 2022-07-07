using Mirror;
using Casino.Poker;

namespace Messages.Client
{
	public class PokerPlayHandMessage : ClientMessage<PokerPlayHandMessage.NetMessage>
	{
		public struct NetMessage : NetworkMessage
		{
			public uint pokerID;
			public PokerDecision decision;
		}

		public override void Process(NetMessage msg)
		{
			var pokerObject = LoadNetworkObject(msg.pokerID);
			var pokerTable = pokerObject.GetComponent<PokerTable>();
			pokerTable.TryPlayHand(msg.decision, SentByPlayer);
		}

		public static void Send(PokerTable pokerTable, PokerDecision newDecision)
		{
			var networkIdentity = pokerTable.GetComponent<NetworkIdentity>();
			NetMessage msg = new NetMessage()
			{
				pokerID = networkIdentity.netId,
				decision = newDecision
			};
			SendToServer(msg);
		}
	}
}