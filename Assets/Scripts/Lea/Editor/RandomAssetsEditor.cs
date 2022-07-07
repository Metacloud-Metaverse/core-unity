using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(RandomAssets))]
public class RandomAssetsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("DO THAT", EditorStyles.miniButton))
        {
            Debug.Log("TEST");
        }
        DrawDefaultInspector();
    }

}
