using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    public InteractableWorld interact;
    private void OnTriggerEnter(Collider other)
    {
        var contoller = other.GetComponent<ThirdPersonController>();
        if (contoller)
        {
            interact.OnInteractRequest(contoller);
        }
    }
 
}
