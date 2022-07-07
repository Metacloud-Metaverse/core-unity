using Casino;
using Messages.Client;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using GLTFast;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SceneChangeRequestMessage : ClientMessage<SceneChangeRequestMessage.NetMessage>
{
	public struct NetMessage : NetworkMessage
	{
		public string sceneName;
		public int instanceNumber;
	}
		
	public override void Process(NetMessage msg)
	{
		SceneLoadManager.Instance.SendPlayerToScene(SentByPlayer, msg.sceneName, msg.instanceNumber);
	}

	public static void Send(string newSceneName, int newInstanceNumber)
	{
		NetMessage msg = new NetMessage()
		{
			sceneName = newSceneName,
			instanceNumber = newInstanceNumber
		};
		SendToServer(msg);
	}
}
