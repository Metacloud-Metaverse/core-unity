using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Mirror;
using Network;

public class LocalPlayerController : NetworkBehaviour
{

	public CinemachineVirtualCamera cinemachineVirtualCamera;
	public ThirdPersonController thirdPersonController;
	public Camera playerCamera;
	public LocalNetworkRootObject[] _rootObjects;

	private void Awake()
	{
		_rootObjects = FindObjectsOfType<LocalNetworkRootObject>();
	}

	void OnValidate()
	{
		thirdPersonController.enabled = false;
		playerCamera.gameObject.SetActive(false);
		cinemachineVirtualCamera.gameObject.SetActive(false);
	//	thirdPersonController.components.rb.constraints |= RigidbodyConstraints.FreezePosition;
	}

	public override void OnStartLocalPlayer()
	{
		AssignLocalSettings();
	}

	/// <summary>
	/// Set the client to control this character
	/// </summary>
	public void AssignLocalSettings()
	{
		Camera.main.enabled = false;
		CustomNetworkManager.localPlayerController = this;
		TogglePlayerUsage(true);
	}

	public void TogglePlayerUsage(bool value)
	{
		//camera
		cinemachineVirtualCamera.gameObject.SetActive(value);
		playerCamera.gameObject.SetActive(value);
		
		TogglePlayerMovement(value);
		
		if (value)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	public void TogglePlayerMovement(bool value)
	{
		thirdPersonController.enabled = value;
	}



}
