using System.Collections;
using Casino.Networking;
using Network;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class OrchestralManager : MonoBehaviourSingleton<OrchestralManager>
{

	public UnityAction OnGameStart;


	public void Start()
	{
		var sceneController = SceneLoadManager.sceneDictionary[SceneLoadManager.firstLoadedScene];
		if (sceneController.sceneType == SceneType.FirstMenu)
		{
			if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null)
			{
				//headless
				FirstScreenManager.Instance.SkipFirstScreen();
			}
			else
			{
				Client.Instance.Initialize();
			}
		}
		else
		{
			StartMenu.Instance.ToggleMenu(true);
		}
	}
	
	
	public void GameStarted()
	{
		OnGameStart.Invoke();
	}


}