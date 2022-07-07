using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Rotator : NetworkBehaviour
{
    public float forceEuler = 300;
    public float forceToPlayer = 100;

    void FixedUpdate()
    {
       transform.eulerAngles += new Vector3(0, 1 * forceEuler * Time.deltaTime, 0);
    }
  
    private void OnCollisionEnter(Collision collision)
    {
        var controller = collision.transform.GetComponent<ThirdPersonController>();
        if (controller)
        {
            var dir = controller.transform.position -collision.contacts[0].point;
            Debug.Log("DIR : " + dir);
            dir.y = -0.1f;
            controller.physics.AddForce(dir, forceToPlayer);
        }
    }
}
