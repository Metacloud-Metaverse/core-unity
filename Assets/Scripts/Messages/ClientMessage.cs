using Mirror;

namespace Messages.Client
{
	public abstract class ClientMessage<T> : GameMessageBase<T> where T : struct, NetworkMessage
	{
		/// <summary>
		/// Player that sent this ClientMessage.
		/// </summary>
		public ConnectedPlayer SentByPlayer;
		public override void Process(NetworkConnection sentBy, T msg)
		{
			SentByPlayer = PlayersManager.Instance.GetPlayer(sentBy);
			base.Process(sentBy, msg);
		}

		public static void SendToServer(T msg)
		{
			NetworkClient.Send(msg, 0);
		}
		
	}
}