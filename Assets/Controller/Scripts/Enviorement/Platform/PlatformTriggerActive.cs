using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlatformTriggerActive : MonoBehaviour
{
    public GameObject triggerObject;
    public Platform activePlatform;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == triggerObject)
        {
            activePlatform.ActivePlatform();
        }
    }
}
