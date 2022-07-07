using Mirror;

namespace Messages.Server
{
	/// <summary>
	/// Represents a network message sent from the server to clients.
	/// Sending a message will invoke the Process() method on the client.
	/// </summary>
	public abstract class ServerMessage<T> : GameMessageBase<T> where T : struct, NetworkMessage
	{
		public static void SendToAllClients(T msg, int channel = 0)
		{
			NetworkServer.SendToAll(msg, channel);
		}

		public static void SendToAllInInstance(T msg, int channel = 0)
		{
			
		}
		
		public static void SendToClient(NetworkConnection recipient, T msg, int channel = 0)
		{
			recipient.Send(msg, channel);
		}

		public static void SendToAllClientsExcept(NetworkConnection exception, T msg, int channel = 0)
		{
			foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
			{
				if (conn.connectionId == exception.connectionId)
				{
					continue;
				}
				conn.Send(msg, channel);
			}
		}
	}
}
