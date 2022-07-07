using System.Collections.Generic;
using Mirror;
using Casino.Poker;
using UnityEngine;

namespace Messages.Client
{
	public class RequestLatejoinUpdate : ClientMessage<RequestLatejoinUpdate.NetMessage>
	{
		public struct NetMessage : NetworkMessage
		{
			public List<uint> netIDList;
		}

		public override void Process(NetMessage msg)
		{
			var networkIdentities = LoadMultipleObjects(msg.netIDList);
			foreach (var networkIdentity in networkIdentities)
			{
				if (networkIdentity != null)
				{
					var requestInterface = (iNeedsServerUpdate)networkIdentity.GetComponent(typeof(iNeedsServerUpdate));
					requestInterface.SendServerUpdate(SentByPlayer);
				}
			}
		}

		public static void Send(List<uint> newNetIDList)
		{
			NetMessage msg = new NetMessage()
			{
				netIDList = newNetIDList
			};
			SendToServer(msg);
		}
	}
}