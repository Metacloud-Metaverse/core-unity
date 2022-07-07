using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItweenGenericUtil utilSizeOnEnter;
    public ItweenGenericUtil utilSizeOnExit;
    public bool automaticCallOff = true;
    public GameObject target;
    private Vector3 _initSize;

    private bool _isAnimActived = false;

    private Hashtable _hasOnEnter;
    private Hashtable _hasOnExit;
    private void Awake()
    {
        _isAnimActived = false;
        _initSize = transform.localScale;
        GenerateHash();
    }

    private void GenerateHash()
    {
        _hasOnEnter = utilSizeOnEnter.GetHas();
        _hasOnExit = utilSizeOnExit.GetHas();
        
        _hasOnEnter.Add("onCompleteTarget",gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GenerateHash();
        if (_isAnimActived == false)
        {
            if (automaticCallOff == false)
            {
                iTween.Stop(gameObject);
                iTween.ScaleTo(target,_hasOnEnter);
            }
            else
            {
                iTween.ScaleTo(target,_hasOnEnter);
            }
            _isAnimActived = true;
        }
    }

    public void OnAnimationCompleted()
    {
        if(automaticCallOff)
          iTween.ScaleTo(target,_hasOnExit);
    }

    public void OnAnimationCompletedEnd()
    {
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        GenerateHash();
        if (automaticCallOff == false)
        {
            iTween.Stop(gameObject);
            iTween.ScaleTo(target,_hasOnExit);
        }
        _isAnimActived = false;
    }
}
