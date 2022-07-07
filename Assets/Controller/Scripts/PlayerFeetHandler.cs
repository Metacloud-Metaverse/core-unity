using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Messages.Server;

public class PlayerFeetHandler : MonoBehaviour
{
    public float groundRadius = 0.2f;
    public ThirdPersonController controller;
    public float offset;
    public bool isGrounded;


    // Update is called once per frame
    void Update()
    {
        var spherePosition = transform.position;
        spherePosition.y -= offset;

        var hits = Physics.OverlapSphere(transform.position, groundRadius, controller.physics.ignorePlayerLayer.value);


        foreach (var item in hits)
        {
            if (item.isTrigger == false)
            {
                if (isGrounded == false)
                {
                    SpawnPoofMessage.Send(transform.position);                
                }
                isGrounded = true;
              //  _controller.animationHandler.isGround = isGrounded;

                return;
            }
        }
        isGrounded = false;
   //     _controller.animationHandler.isGround = isGrounded;
        //   isGrounded = hits.Length >= 1;

    }
    private void OnDrawGizmos()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1f, 0.12f, 0f, 0.55f);

        if (isGrounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - offset, transform.position.z), groundRadius);
    }
}
