using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTestObj : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("Bye", 1f);
    }
    public void Bye()
    {
        gameObject.SetActive(false);
    }
}
