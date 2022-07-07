using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerControllerActionJump 
{
    /*private ThirdPersonController _controller;
    private Transform transform;

    public float jumpForce = 450;


    private float counterCheckJumpEnd;
    private float delayToCheckJumpEnd = 0.05f;
    public bool isJumping { get; private set; }
    public void Init(ThirdPersonController controller)
    {
        _controller = controller;
        _controller.input.OnJumpPress += Jump;
        transform = _controller.transform;
        _controller.StartCoroutine(Update());
     //   _controller.physic.OnGroundHit += OnGroundHit;
    }
    public void OnDestroy()
    {
        _controller.input.OnJumpPress -= Jump;

        //   _controller.physic.OnGroundHit -= OnGroundHit;
    }

    private IEnumerator Update()
    {
        while (true)
        {

            if (isJumping)
            {
                counterCheckJumpEnd += Time.deltaTime;
                if (counterCheckJumpEnd >= delayToCheckJumpEnd && _controller.physic.IsGrounded)
                {
                    counterCheckJumpEnd = 0;
                    isJumping = false;
                }
            }
            yield return null;
        }
    }
  
    private void Jump()
    {
        Debug.Log("Jump");
        if (!_controller.physic.IsGrounded) return;
        if (isJumping) return;
        _controller.components.rb.AddForce(0, jumpForce, 0);
        isJumping = true;
    }
    private void OnGroundHit()
    {
        Debug.Log("On Ground Hit");
        isJumping = false;
    }
    */
}
