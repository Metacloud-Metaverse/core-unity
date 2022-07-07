using System.Collections;
using System.Collections.Generic;
using Mirror;
using Network;
using UnityEngine;

public class AnimationMachine
{
    public delegate void Hook();

    private List<Hook> _hooks = new List<Hook>();
    private GameObject _animationParent;

    public AnimationState[] states;
    private Animation _animation;
    public Animation animation
    {
        get
        {
            if (_animation == null)
                _animation = _animationParent.GetComponentInChildren<Animation>();

            return _animation;
        }
    }
    private AnimationState _currentState;
    public AnimationState currentState { get { return _currentState; } }
    
  
    public AnimationMachine(Animation animation, GameObject animationParent)
    {
        _animation = animation;
        _animationParent = animationParent;

        states = new AnimationState[animation.GetClipCount()];
        for (int i = 0; i < states.Length; i++)
        {
            states[i] = new AnimationState();
        }
    }

    public void Start()
    {
        _currentState = states[0];
        animation.CrossFade(_currentState.animationName);
    }

    public void AddHook(Hook hook)
    {
        _hooks.Add(hook);
    }

    public void RemoveHook(Hook hook)
    {
        _hooks.Remove(hook);
    }
    
    public void ChangeAnimation(string animationString, float fadeDuration)
    {
        animation.CrossFade(animationString, fadeDuration);
    }

    public void Update()
    {
        foreach (var transition in _currentState.transitions)
        {
            if (transition.CheckConditions())
            {
                if (_currentState != null && _currentState != transition.target)
                {
                    transition.target.OnStateEnter();
                }
                _currentState = transition.target;
        
                ChangeAnimation(_currentState.animationName, transition.fadeDuration);

                if (CustomNetworkManager.localPlayerController.thirdPersonController.sendNetworkMessages)
                {
                    ClientAnimatorMessage.Send(_currentState.animationName, transition.fadeDuration);
                }
            }
        }
    }
}
