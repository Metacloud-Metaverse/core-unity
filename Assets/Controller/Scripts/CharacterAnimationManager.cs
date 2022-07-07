using UnityEngine;

public class CharacterAnimationManager : MonoBehaviour
{
    private const int _STATE_IDLE = 0;
    private const int _STATE_WALK = 1;
    private const int _STATE_RUN = 2;
    private const int _STATE_JUMP = 3;
    public string currentAnimation { get { return animationMachine.currentState.animationName; } }

    private ThirdPersonController _controller;
    public AnimationMachine animationMachine { get; private set; }

    public void Awake()
    {
        _controller = GetComponent<ThirdPersonController>();
        animationMachine = new AnimationMachine(_controller.components.anim, gameObject);

        SetAnimationStateNames();

        SetIdleTransitions();
        SetRunTransitions();
        SetJumpTransitions();
        SetWalkTransitions();

        animationMachine.Start();
    }

    public void OnAnimationChange(AnimationMachine.Hook hook)
    {
        animationMachine.AddHook(hook);
    }

    private void SetAnimationStateNames()
    {
        animationMachine.states[_STATE_IDLE].animationName = "Idle";
        animationMachine.states[_STATE_WALK].animationName = "Walk";
        animationMachine.states[_STATE_RUN].animationName  = "Run";
        animationMachine.states[_STATE_JUMP].animationName = "Jump";
    }

    private int _idleTransitionCount = 3;
    private const int _IDLE_TO_WALK = 0;
    private const int _IDLE_TO_RUN = 1;
    private const int _IDLE_TO_JUMP = 2;

    private void SetIdleTransitions()
    {
        var state = animationMachine.states[_STATE_IDLE];
        state.CreateTransitions(_idleTransitionCount);

        state.transitions[_IDLE_TO_WALK].target = animationMachine.states[_STATE_WALK];
        var idleToWalkConditions = state.transitions[_IDLE_TO_WALK].conditions;
        idleToWalkConditions.Add(CheckWalkingModeActive);
        idleToWalkConditions.Add(CheckVelocityGreaterThanZero);
        idleToWalkConditions.Add(CheckGrounded);

        state.transitions[_IDLE_TO_RUN].target = animationMachine.states[_STATE_RUN];
        var idleToRunConditions = state.transitions[_IDLE_TO_RUN].conditions;
        idleToRunConditions.Add(CheckWalkingModeUnactive);
        idleToRunConditions.Add(CheckVelocityGreaterThanZero);
        idleToRunConditions.Add(CheckGrounded);

        state.transitions[_IDLE_TO_JUMP].target = animationMachine.states[_STATE_JUMP];
        state.transitions[_IDLE_TO_JUMP].fadeDuration = 0.1f;
        var idleToJumpConditions = state.transitions[_IDLE_TO_JUMP].conditions;

        idleToJumpConditions.Add(CheckJumping);
//        idleToJumpConditions.Add(CheckGrounded);

    }

    private int _runTransitionCount = 3;
    private const int _RUN_TO_IDLE = 0;
    private const int _RUN_TO_WALK = 1;
    private const int _RUN_TO_JUMP = 2;

    private void SetRunTransitions()
    {
        var state = animationMachine.states[_STATE_RUN];
        state.CreateTransitions(_runTransitionCount);

        state.transitions[_RUN_TO_IDLE].target = animationMachine.states[_STATE_IDLE];
        var runToIdleConditions = state.transitions[_RUN_TO_IDLE].conditions;
        runToIdleConditions.Add(CheckVelocityEqualToZero);
        runToIdleConditions.Add(CheckGrounded);

        state.transitions[_RUN_TO_WALK].target = animationMachine.states[_STATE_WALK];
        var runToWalkConditions = state.transitions[_RUN_TO_WALK].conditions;
        runToWalkConditions.Add(CheckVelocityGreaterThanZero);
        runToWalkConditions.Add(CheckGrounded);
        runToWalkConditions.Add(CheckWalkingModeActive);

        state.transitions[_RUN_TO_JUMP].target = animationMachine.states[_STATE_JUMP];
        var runToJumpConditions = state.transitions[_RUN_TO_JUMP].conditions;
        runToJumpConditions.Add(CheckGrounded);
        runToJumpConditions.Add(CheckJumping);
        state.transitions[_RUN_TO_JUMP].fadeDuration = 0.1f;
    }

    private int _jumpTransitionCount = 3;
    private const int _JUMP_TO_IDLE = 0;
    private const int _JUMP_TO_WALK = 1;
    private const int _JUMP_TO_RUN = 2;

    private void SetJumpTransitions()
    {
        var state = animationMachine.states[_STATE_JUMP];
        state.CreateTransitions(_jumpTransitionCount);

        state.transitions[_JUMP_TO_IDLE].target = animationMachine.states[_STATE_IDLE];
        var jumpToIdle = state.transitions[_JUMP_TO_IDLE].conditions;
        jumpToIdle.Add(CheckVelocityEqualToZero);
        jumpToIdle.Add(CheckGrounded);

        state.transitions[_JUMP_TO_RUN].target = animationMachine.states[_STATE_RUN];
        var jumpToRun = state.transitions[_JUMP_TO_RUN].conditions;
        jumpToRun.Add(CheckVelocityGreaterThanZero);
        jumpToRun.Add(CheckGrounded);
        jumpToRun.Add(CheckNotJumping);
        jumpToRun.Add(CheckWalkingModeUnactive);
        state.transitions[_JUMP_TO_RUN].fadeDuration = 0.1f;

        state.transitions[_JUMP_TO_WALK].target = animationMachine.states[_STATE_WALK];
        var jumpToWalk = state.transitions[_JUMP_TO_WALK].conditions;
        jumpToWalk.Add(CheckVelocityGreaterThanZero);
        jumpToWalk.Add(CheckGrounded);
        jumpToWalk.Add(CheckNotJumping);
        jumpToWalk.Add(CheckWalkingModeActive);
        state.transitions[_JUMP_TO_WALK].fadeDuration = 0.1f;
    }

    private int _walkTransitionsCount = 3;
    private const int _WALK_TO_IDLE = 0;
    private const int _WALK_TO_RUN = 1;
    private const int _WALK_TO_JUMP = 2;

    private void SetWalkTransitions()
    {
        var state = animationMachine.states[_STATE_WALK];
        state.CreateTransitions(_walkTransitionsCount);

        var walkToIdle = state.transitions[_WALK_TO_IDLE];
        walkToIdle.target = animationMachine.states[_STATE_IDLE];
        walkToIdle.conditions.Add(CheckVelocityEqualToZero);
        walkToIdle.conditions.Add(CheckGrounded);

        var walkToRun = state.transitions[_WALK_TO_RUN];
        walkToRun.target = animationMachine.states[_STATE_RUN];
        walkToRun.conditions.Add(CheckVelocityGreaterThanZero);
        walkToRun.conditions.Add(CheckGrounded);
        walkToRun.conditions.Add(CheckWalkingModeUnactive);

        var walkToJump = state.transitions[_WALK_TO_JUMP];
        walkToJump.target = animationMachine.states[_STATE_JUMP];
        walkToJump.conditions.Add(CheckGrounded);
        walkToJump.conditions.Add(CheckJumping);
        walkToJump.fadeDuration = 0.1f;
    }

    private bool CheckJumping()
    {
        return _controller.animationHandler.isJumping;
    }

    private bool CheckNotJumping()
    {
        return !_controller.animationHandler.isJumping;
    }

    private bool CheckWalkingModeActive()
    {
        return _controller.input.sprint;
    }

    private bool CheckWalkingModeUnactive()
    {
        return !_controller.input.sprint;
    }

    private bool CheckVelocityGreaterThanZero()
    {
        return(_controller.animationHandler.moveSpeed != 0) ? true : false;
    }

    private bool CheckVelocityEqualToZero()
    {
        return (_controller.animationHandler.moveSpeed == 0) ? true : false;
    }

    private bool CheckGrounded()
    {
        return _controller.animationHandler.isGround;
    }

    public void Update()
    {
        animationMachine.Update();
    }
}
