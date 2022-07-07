using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalNetworkRootObject : MonoBehaviour
{
    public GameObject root;

    public void Active()
    {
        Debug.Log("Active");
        root.SetActive(true);
    }
}
