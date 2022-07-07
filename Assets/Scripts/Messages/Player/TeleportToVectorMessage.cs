using Messages.Server;
using Mirror;
using Network;
using System.Collections;
using System.Collections.Generic;
using Casino.Networking;
using Mirror;
using Network;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportToVectorMessage : ServerMessage<TeleportToVectorMessage.NetMessage>
{
	public struct NetMessage : NetworkMessage
	{
		public Vector3 value;
	}
		
	public override void Process(NetMessage msg)
	{
		CustomNetworkManager.localPlayerController.thirdPersonController.gameObject.transform.position = msg.value;
	}

	public static void Send(NetworkIdentity networkIdentity, Vector3 newValue)
	{
		NetMessage msg = new NetMessage()
		{
			value = newValue,
		};
		SendToClient(networkIdentity.connectionToClient, msg);
	}
}