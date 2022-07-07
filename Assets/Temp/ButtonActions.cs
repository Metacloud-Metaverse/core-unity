using System.Collections;
using System.Collections.Generic;
using Network;
using UnityEngine;

public class ButtonActions : MonoBehaviour
{
    public string sceneName;
    public int instanceTarget;
    public void Teleport()
    {
        SceneChangeRequestMessage.Send(sceneName, instanceTarget);
        CustomNetworkManager.localPlayerController.playerCamera.gameObject.SetActive(true);
    }

    public void OnButtonHouse()
    {
        EnterHouseRequestMessage.Send();
    }
}
