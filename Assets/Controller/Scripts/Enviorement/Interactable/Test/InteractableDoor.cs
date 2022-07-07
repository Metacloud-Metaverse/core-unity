using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : InteractableWorld
{
    public Transform openPos;
    public Transform closePos;

    private Vector3 _target;
    public float moveSpeed = 5;
    private Coroutine moveCorrutine;

    public override void NetworkMe()
    {
        OpenDoor();
    }
    private void OpenDoor()
    {
        if (_target != closePos.position)
            _target = closePos.position;
        else
            _target = openPos.position;

        if (moveCorrutine != null)
            StopCoroutine(moveCorrutine);

        moveCorrutine = StartCoroutine(MoveTo(_target));

       // _identity.netId;
    }
    private IEnumerator MoveTo(Vector3 pos)
    {
        while (transform.position != pos)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

}
