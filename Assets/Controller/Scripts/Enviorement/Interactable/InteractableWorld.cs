using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class InteractableWorld : MonoBehaviour
{
    public PopupCursorData popupMouseData;
    private int _highlightMaskLayer;
    private int _defaultMaskLayer;

    protected ThirdPersonController _controller;

    protected NetworkIdentity _identity;
    public void Awake()
    {
        _identity = GetComponent<NetworkIdentity>();
        _highlightMaskLayer = LayerMask.NameToLayer("HighLight");
        _defaultMaskLayer = LayerMask.NameToLayer("Default");
    }
    public void OnInteractRequest(ThirdPersonController controller)
    {
        _controller = controller;
        NetworkMe();
    }
    public abstract void NetworkMe();


    public void OnInteractEnter()
    {
        gameObject.layer = _highlightMaskLayer;
        if(popupMouseData)
            PopupCursor.Instance.OpenMousePopup(popupMouseData.sprite,popupMouseData.text);
    }
    public void OnInteractExit()
    {
        gameObject.layer = _defaultMaskLayer;
        if(popupMouseData)
            PopupCursor.Instance.CloseMousePopup();
    }
}
