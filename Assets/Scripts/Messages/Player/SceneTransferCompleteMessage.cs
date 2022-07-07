using System.Linq;
using Casino.Networking;
using Messages.Client;
using Messages.Server;
using Mirror;
using Network;
using UnityEngine;

public class SceneTransferCompleteMessage : ClientMessage<SceneTransferCompleteMessage.NetMessage>
{
	public struct NetMessage : NetworkMessage {}
		
	public override void Process(NetMessage msg)
	{
		var playerNetworkIdentity = SentByPlayer.networkConnection.identity;
		var sceneController = SceneLoadManager.sceneDictionary[playerNetworkIdentity.gameObject.scene];

		SentByPlayer.currentSceneType = sceneController.sceneType;
		
		var spawnPoint = new Vector3();
		if (sceneController.customStartPositionList.Count > 0)
		{
			var customStartPosition = sceneController.customStartPositionList[Random.Range(0, sceneController.customStartPositionList.Count)];
			spawnPoint = customStartPosition.transform.position;
		}
		TeleportToVectorMessage.Send(playerNetworkIdentity, spawnPoint);
		
		//networked objects in the scene
		//TODO: REALLY poor performance;
		foreach (var networkIdentity in NetworkServer.spawned.Values)
		{
			NetworkServer.RebuildObservers(networkIdentity, false);
		}
	}

	public static void Send()
	{
		NetMessage msg = new NetMessage();
		SendToServer(msg);
	}
}