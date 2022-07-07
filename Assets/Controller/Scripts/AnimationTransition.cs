using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTransition
{
    public delegate bool Condition();

    public List<Condition> conditions = new List<Condition>();

    public AnimationState target;
    public float fadeDuration = 0.3f;

    public bool CheckConditions()
    {
        if (conditions.Count == 0) return false;

        foreach (var condition in conditions)
        {
            if (!condition()) return false;
        }
        return true;
    }
}
