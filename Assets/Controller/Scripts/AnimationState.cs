using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationState
{
    public AnimationTransition[] transitions;

    public string animationName;

    public void CreateTransitions(int count)
    {
        transitions = new AnimationTransition[count];
        for (int i = 0; i < transitions.Length; i++)
        {
            transitions[i] = new AnimationTransition();
        }
    }

    public void OnStateEnter()
    {
        
    }
}
