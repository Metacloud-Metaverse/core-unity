using Messages.Server;
using Mirror;
using Network;
using UnityEngine;

public class ServerVisualAndEffectsMessage : ServerMessage<ServerVisualAndEffectsMessage.NetMessage>
{
	public struct NetMessage : NetworkMessage
	{
		public string value;
		public float fadeDuration;
		public uint characterID;
	}
		
	public override void Process(NetMessage msg)
	{
		var character= LoadNetworkObject(msg.characterID);
		var localPlayerController = character.GetComponent<LocalPlayerController>();
		if (CustomNetworkManager.localPlayerController == localPlayerController)
		{
			return; //dont let the server update our own movement. TODO: dont send the message on the first place ( SendToAllClientsExcept() ? )
		}
		if (localPlayerController.thirdPersonController)
		{
			localPlayerController.thirdPersonController.animationManager.animationMachine.ChangeAnimation(msg.value, msg.fadeDuration);
		}
	}

	public static void Send(LocalPlayerController localPlayerController, string newValue, float newFadeDuration)
	{
		var networkIdentity = localPlayerController.GetComponent<NetworkIdentity>();
		NetMessage msg = new NetMessage()
		{
			value = newValue,
			fadeDuration = newFadeDuration,
			characterID = networkIdentity.netId,
		};
		SendToAllClients(msg);
	}
}