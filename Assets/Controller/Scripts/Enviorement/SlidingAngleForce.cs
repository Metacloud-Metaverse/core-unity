using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingAngleForce : MonoBehaviour
{
    public float force = 5;
    private void OnTriggerEnter(Collider other)
    {
        var controller = other.GetComponent<ThirdPersonController>();
        if (controller)
        {
            controller.physics.SetExternalSlidingForce(force);
        }
    }
    private void OnTriggerExit(Collider other)
    {

        var controller = other.GetComponent<ThirdPersonController>();
        if (controller)
        {
            controller.physics.SetExternalSlidingForce(0);
        }
    }
}
