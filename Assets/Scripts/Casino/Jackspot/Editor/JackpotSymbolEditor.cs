using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JackpotSymbol))]
public class JackpotSymbolEditor : Editor
{
    private JackpotSymbol myTarget;
    private SerializedProperty rewardList;

    private void OnEnable()
    {
        myTarget = (JackpotSymbol)target;
        rewardList = serializedObject.FindProperty("reward");
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        myTarget.symbolName = (JackpotSymbolName)EditorGUILayout.EnumPopup("Name: ", myTarget.symbolName);
        myTarget.type = (SymbolType)EditorGUILayout.EnumPopup("Type: ", myTarget.type);
        myTarget.prfab = (GameObject)EditorGUILayout.ObjectField("Prefab :", myTarget.prfab, typeof(GameObject), true);
        DrawFrecuency();
        EditorGUILayout.PropertyField(rewardList);

        EditorGUILayout.EndVertical();
    }

    private void DrawFrecuency()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        if (GUILayout.Button("Frecuency"))
        {
            myTarget.showFrecuencyEditor = !myTarget.showFrecuencyEditor;
        }
        if (myTarget.showFrecuencyEditor)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            myTarget.frequency = EditorGUILayout.IntField("Frecuency : ", myTarget.frequency);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
    }
    private void OnDisable()
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(myTarget);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
    }
}
