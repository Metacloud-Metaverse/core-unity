using Casino.Networking;
using UnityEngine;
using Mirror;

/// <summary>
/// Server-only full player information class
/// </summary>
public class ConnectedPlayer
{
	public NetworkConnectionToClient networkConnection;
	public bool inOpenWorld;
	public SceneType currentSceneType;

	public int instanceNumber;
}
