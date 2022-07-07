using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveGameSceneButton : MonoBehaviour
{
	public void LeaveGameScene()
	{
		SceneChangeRequestMessage.Send("Parcel Loading Test", 0);
		var sceneController = SceneLoadManager.sceneDictionary[gameObject.scene];
		sceneController.sceneCanvas.enabled = false;
		sceneController.camera.gameObject.SetActive(false);
	}
}
