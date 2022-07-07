using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    //viejos para borrar
    /* public PlayerControllerComponents components;
      public PlayerControllerPhysics physic;
      public PlayerControllerActionMovement movement;
      public PlayerControllerActionJump jump;
      public PlayerControllerInput input;*/

    public PlayerControllerStateHandler state;
    public PlayerControllerPhysicsHandler physics;
    public PlayerControllerInput input;
    public PlayerControllerJumpHandler jump;
    public PlayerControllerMovementHandler move;
    public PlayerControllerAnimationHandler animationHandler;
    public PlayerControllerCameraHandler cameraHandler;
    public PlayerControllerRaycastHandler raycastsHandler;
    public PlayerControllerCursorHandler cursorHandler;
    public PlayerControllerEffects effects;
    public PlayerComponents components;

    public bool sendNetworkMessages;

    public CharacterAnimationManager animationManager;

    private bool isInitialized = false;
    
    

    private void OnEnable()
    {
        if (isInitialized)
            return;
        
        InitializateComponents();
        isInitialized = true;
        physics.OnGroundEnter += SendStartmenuProcces;
    }

    private void SendStartmenuProcces()
    {
        StartMenu.Instance.ReportProgresLoad();
        physics.OnGroundEnter -= SendStartmenuProcces;
        Debug.Log("On Ground EnterPlayer");

    }

   
    private void OnDestroy()
    {
        effects.OnDestroy();
    }

    private void InitializateComponents()
    {
        state.Init(this);
        cursorHandler.Init(this);
        input.Init(this);
        raycastsHandler.Init(this);
        physics.Init(this);
        cameraHandler.Init(this);
        move.Init(this);
        jump.Init(this);
        animationHandler.Init(this);
        effects.Init(this);
        SetEnablePlayerHandlers(true);
        
        
    }
    public void OnLocalPlayerStart()
    {
        this.enabled = true;
        components.camera.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Start()
    {
        if (sendNetworkMessages)
            animationManager.OnAnimationChange(SyncAnimations);
    }
    public void SetEnablePlayerHandlers(bool value)
    {
        input.SetHandlerEnable(value);
        jump.SetHandlerEnable(value);
        physics.SetHandlerEnable(value);
    }
    public void ChangeAllAnimSpeed(float newSpeed)
    {
        foreach (var item in animationManager.animationMachine.states)
        {
            components.anim[item.animationName].speed = newSpeed;
        }
    }

    private void SyncAnimations()
    {
        
    }
    private void OnDrawGizmos()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1f, 0.12f, 0f, 0.55f);

        if (physics.isGrounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - physics.groundOffset, transform.position.z), physics.groundRadius);
    }

    void Update()
    {
        raycastsHandler.Update();
        animationManager.Update();       
    }

  
}