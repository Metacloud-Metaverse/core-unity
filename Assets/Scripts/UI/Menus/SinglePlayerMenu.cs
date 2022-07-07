using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SinglePlayerMenu : MonoBehaviour
{
	private bool mouseFocus;
	
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (mouseFocus == false)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
			else
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			mouseFocus = !mouseFocus;
		}
	}
	
	public void ChangeScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
		UIManager.Instance.startMenu.ToggleMenu(true);
	}
	

}
