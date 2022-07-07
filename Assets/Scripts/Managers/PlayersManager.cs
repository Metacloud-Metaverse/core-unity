using System.Collections.Generic;
using Mirror;

public class PlayersManager : MonoBehaviourSingleton<PlayersManager>
{
	
	public List<ConnectedPlayer> playerList = new List<ConnectedPlayer>();
	
	public ConnectedPlayer GetPlayer(NetworkConnection sentBy)
	{
		foreach (var connectedPlayer in playerList)
		{
			if (connectedPlayer.networkConnection == sentBy)
			{
				return connectedPlayer;
			}
		}
		return null;
	}
}
