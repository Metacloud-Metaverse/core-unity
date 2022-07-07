using System;
using System.Collections;
using System.Collections.Generic;
using Network;
using UnityEngine;
using UnityEngine.Events;
public class TeleportButton : MonoBehaviour
{
    public GameObject feedbackOnMouseEnterObject;
    public PopupCursorData popupMouseData;
    public UnityEvent onButtonClick;
    private void Awake()
    {
    }

    void OnMouseDown()
    {
        if (onButtonClick != null)
        {
            onButtonClick.Invoke();
        }

    }

    private void OnMouseEnter()
    {
        feedbackOnMouseEnterObject.SetActive(true);
        if(popupMouseData)
            PopupCursor.Instance.OpenMousePopup(popupMouseData.sprite,popupMouseData.text);
    }

    private void OnMouseExit()
    {
        feedbackOnMouseEnterObject.SetActive(false);
        if(popupMouseData)
            PopupCursor.Instance.CloseMousePopup();
    }
}
