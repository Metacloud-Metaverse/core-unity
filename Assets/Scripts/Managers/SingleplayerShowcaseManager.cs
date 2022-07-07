using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SingleplayerShowcaseManager : MonoBehaviourSingleton<SingleplayerShowcaseManager>
{
	public GameObject playerPrefab;
	public GameObject sceneMenuPrefab;
	
	public void SpawnSingleplayerCharacter()
	{
		var position = new Vector3();
		var spawnPosition = FindObjectOfType<CustomStartPosition>();
		if (spawnPosition != null)
		{
			position = spawnPosition.transform.position;
		}
		var characterGameObject = Instantiate(playerPrefab, position, new Quaternion());
		var localPlayerController = characterGameObject.GetComponent<LocalPlayerController>();
		localPlayerController.AssignLocalSettings();
		
		var sceneMenu = Instantiate(sceneMenuPrefab, new Vector3(), new Quaternion());
		
	}
	
	
}
