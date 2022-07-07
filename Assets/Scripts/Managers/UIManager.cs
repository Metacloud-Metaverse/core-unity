using System.Collections;
using System.Collections.Generic;
using Network;
using UnityEngine;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
	public StartMenu startMenu;
	public SinglePlayerMenu singlePlayerMenu;
	public LoadingScreen loadingScreen;
	public MapMenu mapMenu;
	
	public void ToggleMap()
	{
		var newValue = !mapMenu.gameObject.activeSelf;
		mapMenu.gameObject.SetActive(newValue);
		CustomNetworkManager.localPlayerController.TogglePlayerMovement(!newValue);
	}
}
