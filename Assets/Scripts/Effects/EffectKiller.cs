using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectKiller : MonoBehaviour
{
    public KillerType type = KillerType.Disable;
    public float time;
    private void OnEnable()
    {
        StartCoroutine(Desactive());
    }
    private IEnumerator Desactive()
    {
        switch (type)
        {
            case KillerType.Disable:
                yield return Yielders.Seconds(time);
                DisableObject();
                break;
            case KillerType.Destroy:
                yield return Yielders.Seconds(time);
                DestroyObject();
                break;
        }
    }
    private void DisableObject()
    {
        gameObject.SetActive(false);
    }
    private void DestroyObject()
    {
        gameObject.SetActive(false);
    }
}

public enum KillerType
{
    Disable,
    Destroy,
}
