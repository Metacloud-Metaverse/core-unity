using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HelpTextureSize : EditorWindow
{
    int height, widht;
    bool isMultipleHeight;
    bool isMultipleWidht;
    [MenuItem("Tools/TextureSize")]
    public static void TextureHelper()
    {
        HelpTextureSize HelpTextureSize = (HelpTextureSize)EditorWindow.GetWindow(typeof(HelpTextureSize));
        HelpTextureSize.Show();
    }

    void OnGUI()
    {
        height =EditorGUILayout.IntField("Height :", height);
        widht = EditorGUILayout.IntField("Widht :", widht);


        if (GUILayout.Button("CheckMultiple"))
        {
            isMultipleHeight = false;
            isMultipleWidht = false;
            if (height % 4 == 0)
                isMultipleHeight = true;
            if (widht % 4 == 0)
                isMultipleWidht = true;
        }
        GUILayout.Label("Height : "+ isMultipleHeight.ToString(), EditorStyles.boldLabel);
        GUILayout.Label("Widht : "+ isMultipleWidht.ToString(), EditorStyles.boldLabel);


    }
}
