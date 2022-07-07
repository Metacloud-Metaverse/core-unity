using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PopupCursorData",menuName = "ScriptableObject/UI/CursorData")]
public class PopupCursorData : ScriptableObject
{
    public Sprite sprite;
    [TextArea] public string text;
}
