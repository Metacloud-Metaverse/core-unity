using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetworkLocalPlatform : MonoBehaviour
{
    public GameObject platform;
    
    public void Active()
    {
        Debug.Log("asdasdasd");
        platform.SetActive(true);
    }
   
}
