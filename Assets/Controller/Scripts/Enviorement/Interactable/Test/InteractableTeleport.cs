using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTeleport : InteractableWorld
{
    public Transform toPosition;
    public bool stopPlayerMovement = true;

    public override void NetworkMe()
    {
        StartCoroutine(Teleport());
    }
    private IEnumerator Teleport()
    {
        if(stopPlayerMovement)
            _controller.move.StopForce(0.5f);

        _controller.components.controller.enabled = false;
        while (_controller.transform.position != toPosition.position)
        {
            _controller.transform.position = toPosition.position;
            yield return null;
        }
        _controller.move.StopForce(0.5f);
        _controller.components.controller.enabled = true;
    }

}
