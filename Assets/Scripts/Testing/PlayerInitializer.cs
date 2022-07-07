using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    public LocalPlayerController controller;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        controller.AssignLocalSettings();
        controller.gameObject.SetActive(true);
    }

    
}
