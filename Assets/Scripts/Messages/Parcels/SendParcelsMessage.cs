using System.Collections.Generic;
using Messages.Client;
using Messages.Server;
using Mirror;
using UnityEngine;

public class SendParcelsMessage : ServerMessage<SendParcelsMessage.NetMessage>
{
	private int currecntProcces = 0;
	public struct NetMessage : NetworkMessage
	{
		public List<ParcelInfo> parcelInfoList;
	}
		
	public override void Process(NetMessage msg)
	{
		for (int i = 0; i < msg.parcelInfoList.Count; i++)
		{
			var parcelInfo = msg.parcelInfoList[i];
			if (ParcelManager.Instance.localParcelInfoDic.ContainsKey(parcelInfo.Location))
			{
				//in case we asked for this parcel several times before the message arrived
				msg.parcelInfoList.Remove(parcelInfo);
				continue;
			}
			ParcelManager.Instance.localParcelInfoDic.Add(parcelInfo.Location, parcelInfo);
			if (ParcelManager.Instance.localParcelDic.ContainsKey(parcelInfo.Location) == false)
			{
				ParcelManager.Instance.GenerateParcel(parcelInfo.Location);
			}
			var parcel = ParcelManager.Instance.localParcelDic[parcelInfo.Location];
			parcel.Initialize(parcelInfo);
		}
		StartMenu.Instance.ReportProgresLoad();
	}

	public static void Send(NetworkConnection networkConnection, List<ParcelInfo> NewParcelInfoList)
	{
		NetMessage msg = new NetMessage()
		{
			parcelInfoList = NewParcelInfoList
		};
		SendToClient(networkConnection, msg);
	}
}
