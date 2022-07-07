using Messages.Client;
using Messages.Server;
using Mirror;
using Network;

public class ClientAnimatorMessage : ClientMessage<ClientAnimatorMessage.NetMessage>
{
	public struct NetMessage : NetworkMessage
	{
		public string value;
		public float fadeDuration;
	}
		
	public override void Process(NetMessage msg)
	{
		var localPlayerController = SentByPlayer.networkConnection.identity.GetComponent<LocalPlayerController>();

		ServerVisualAndEffectsMessage.Send(localPlayerController, msg.value, msg.fadeDuration);
	}

	public static void Send(string newValue, float newFadeDuration)
	{
		NetMessage msg = new NetMessage()
		{
			value = newValue,
			fadeDuration = newFadeDuration
		};
		SendToServer(msg);
	}
}