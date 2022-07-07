using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cursor = UnityEngine.Cursor;

[System.Serializable]
public class PlayerControllerCursorHandler
{
    public Sprite triggerUICursor;
    public Sprite normalUICursor;
    public Texture2D triggerCursor;
    public Texture2D normalCursor;
    private ThirdPersonController _controller;

    public Image cursorUI;
    public void Init(ThirdPersonController controller)
    {
        _controller = controller;
        Cursor.SetCursor(normalCursor,Vector2.zero, CursorMode.Auto);
    }

    public void SetUICursorInteractable()
    {
        cursorUI.sprite = triggerUICursor;
    } public void SetUICursorNomal()
    {
        cursorUI.sprite = normalUICursor;
    }


    /// <summary>
    /// Set Cursor on Center overlay UI
    /// </summary>
    public void SetActiveUICursor(bool value)
    {
        cursorUI.gameObject.SetActive(value);
    }
}
