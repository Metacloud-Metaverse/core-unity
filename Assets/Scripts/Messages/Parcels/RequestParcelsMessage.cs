using System.Collections.Generic;
using Messages.Client;
using Mirror;
using UnityEngine;

public class RequestParcelsMessage : ClientMessage<RequestParcelsMessage.NetMessage>
{
	public struct NetMessage : NetworkMessage
	{
		public List<Vector2Int> requestedList;
	}
		
	public override void Process(NetMessage msg)
	{
		var parcelInfoList = new List<ParcelInfo>();
		for (int i = 0; i < msg.requestedList.Count; i++)
		{
			var location = msg.requestedList[i];
			var parcelInfo = ParcelManager.Instance.serverAllParcelInfo[location];

			//not a 1x1 parcel, but a combination of several, we send the whole thing
			if (parcelInfo.parcelPattern != ParcelPattern.alone)
			{
				//already added in a previous loop
				if (parcelInfoList.Contains(parcelInfo) == false)
				{
					var parentParcelInfo = ParcelManager.Instance.serverAllParcelInfo[parcelInfo.parent];
					foreach (var childrenPos in parentParcelInfo.children)
					{
						var childrenParcelInfo = ParcelManager.Instance.serverAllParcelInfo[childrenPos];
						parcelInfoList.Add(childrenParcelInfo);
					}
				}
				continue;
			}

			parcelInfoList.Add(parcelInfo);
		}
		
		//TODO: before sending check the size, if its too large separate the list into chunks and send different messages
		
		SendParcelsMessage.Send(SentByPlayer.networkConnection, parcelInfoList);
	}

	public static void Send(List<Vector2Int> newRequestedList)
	{
		NetMessage msg = new NetMessage()
		{
			requestedList = newRequestedList
		};
		SendToServer(msg);
	}
}
