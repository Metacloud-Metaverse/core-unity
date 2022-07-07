using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviourSingleton<StartMenu>
{

	[SerializeField] private GameObject panelOnBuild;
	[SerializeField] private GameObject panelOnTest;
	[SerializeField] private LoadingScreen panelLoadingMenu;
	public TMP_InputField addressInputField;
	public Button hostButton;
	[SerializeField] private Canvas canvas;
	
	
	//vars for check loadMenu on Start
	private int currentProcces = 0;
	private bool isFirstShowOpenLoadingMenu = false;
	void Start()
	{
		currentProcces = 0;
		isFirstShowOpenLoadingMenu = false;
		addressInputField.text = NetworkManager.singleton.networkAddress;
		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			hostButton.enabled = false;
		}
	}
	

	private void OnEnable()
	{
		panelOnTest.SetActive(true);
		panelOnBuild.SetActive(true);
	}

	public void ReportProgresLoad()
	{
		if (isFirstShowOpenLoadingMenu == false)
		{
			currentProcces++;
			//panelLoadingMenu.SetProgres(currentProcces);

			if(currentProcces >= 3)
			{
				isFirstShowOpenLoadingMenu = true;
			//	panelLoadingMenu.gameObject.SetActive(false);
			//	panelLoadingMenu.SetLoadingBarProgress(currentProcces);
			}

		}
	}

	public void StartHost()
	{
		NetworkManager.singleton.StartHost();
		//panelLoadingMenu.gameObject.SetActive(true);
		ToggleMenu(false);
	}

	public void StartClient()
	{
		NetworkManager.singleton.StartClient();
		ToggleMenu(false);
	}

	public void ChangeNetworkAddress()
	{
		NetworkManager.singleton.networkAddress = addressInputField.text;
	}

	public void SingleplayerShowcase()
	{
		SingleplayerShowcaseManager.Instance.SpawnSingleplayerCharacter();
		ToggleMenu(false);
	}

	public void ToggleMenu(bool value)
	{
		//var canvas = GetComponent<Canvas>();
		canvas.enabled = value;
		//gameObject.SetActive(true);
	}

}
