using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Casino.Networking;

public class CustomStartPosition : MonoBehaviour
{
	private SceneController sceneController;

	public void Start()
	{ 
		sceneController = SceneLoadManager.sceneDictionary[gameObject.scene];
		sceneController.customStartPositionList.Add(this);
		if (sceneController.sceneType == SceneType.Main)
		{
			//login spawnpoint
			NetworkManager.RegisterStartPosition(transform);
		}
	}
	
	public void Awake()
	{

	}

	public void OnDestroy()
	{
		NetworkManager.UnRegisterStartPosition(transform);
	}
}
