using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class PlayerControllerAnimationHandler
{

    private string _lasState;
    public bool isJumping{ get; private set; }
    public bool isGround;
    public bool isFalling{ get; private set; }
    private float _moveBlend;
    public float moveSpeed;
    private ThirdPersonController _player;

    
    public void Init(ThirdPersonController player)
    {
        _player = player;
    }
    private void AssignAnimationIDs()
    {
      /*  _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");*/
    }
    
    public void SetMoveBlend(float value)
    {
        _moveBlend = value;
    }
    public void SetMoveSpeed(float value)
    {
        moveSpeed = value;
    }
    public void SetJump(bool value)
    {
        isJumping = value;
    }
    public void SetFalling(bool value)
    {
        isFalling = value;
    }
    public void SetGround(bool value)
    {
        isGround = value;
    }

}
