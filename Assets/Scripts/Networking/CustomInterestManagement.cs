using System.Collections;
using System.Collections.Generic;
using Casino.Networking;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomInterestManagement : SpatialHashingInterestManagement
{

    public override bool OnCheckObserver(NetworkIdentity identity, NetworkConnectionToClient newObserver)
    {
        var connectedPlayer = PlayersManager.Instance.GetPlayer(newObserver);
        var sceneController = SceneLoadManager.sceneDictionary[identity.gameObject.scene];
        if (connectedPlayer.instanceNumber != identity.instanceNumber)
        {
            return false;
        }
        if ((connectedPlayer.currentSceneType == SceneType.WorldScene || connectedPlayer.currentSceneType == SceneType.Main)
            && (sceneController.sceneType == SceneType.WorldScene || sceneController.sceneType == SceneType.Main))
        {
            //TODO: distance checks
            //var distanceCheck = base.OnCheckObserver(identity, newObserver);
            //return distanceCheck;
        }
        return true;
    }
    
    public override void OnRebuildObservers(NetworkIdentity identity, HashSet<NetworkConnectionToClient> observers)
    {
        foreach (var connectedPlayer in PlayersManager.Instance.playerList)
        {
            if (OnCheckObserver(identity, connectedPlayer.networkConnection))
            {
                observers.Add(connectedPlayer.networkConnection);
            }
        }
    }
}