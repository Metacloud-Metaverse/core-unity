using System;
using System.Collections;
using UnityEngine;
[System.Serializable]
public class PlayerControllerStateHandler 
{
    public PlayerControllerState state;
    private PlayerControllerState _prevState;
    private ThirdPersonController _controller;

    public StateEvents HitGround;
    
    public delegate void StateEvents();

    public void Init(ThirdPersonController controller)
    {
        _controller = controller;
        _controller.StartCoroutine(CustomUpdate());
    }

    private IEnumerator CustomUpdate()
    {
        while (true)
        {
            CheckState();
            yield return null;
        }
    }
    public void CheckState()
    {
        if(_controller.animationHandler.isGround)
        {
            if ( _controller.components.controller.velocity.x == 0
                &&  _controller.components.controller.velocity.z == 0)
            {
                state = PlayerControllerState.Idle;
            }
        }
        if (_controller.animationHandler.isJumping)
        {
            if (_controller.animationHandler.isFalling)
                state = PlayerControllerState.Fall;
            else
                state = PlayerControllerState.Jump;
        }
        else if (_controller.animationHandler.moveSpeed != 0)
        {
            if (!_controller.input.sprint)
            {
                state = PlayerControllerState.Run;
            }
            else
            {
                state = PlayerControllerState.Walk;
            }
        }

        if (_prevState != state)
        {
            if(_prevState == PlayerControllerState.Fall)
            {
                if (_controller.physics.isGrounded)
                {
                    HitGround?.Invoke();
                }
            }
            _prevState = state;
        }
    }
}
