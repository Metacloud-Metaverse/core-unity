
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridChildController))]
public class GridChildControllerEditor : Editor
{
    public GridChildController myTarget;
    private void OnEnable()
    {
        myTarget = (GridChildController)target;
    }
    public override void OnInspectorGUI()
    {

        if (myTarget.childCount != myTarget.transform.childCount)
        {
            myTarget.childCount = myTarget.transform.childCount;
            myTarget.GetChildObjects();
        }
        GUILayout.Label("Grid Child :   Childs :" + myTarget.childCount, EditorStyles.centeredGreyMiniLabel);

        #region Size
        GUILayout.BeginVertical(EditorStyles.helpBox);
        if (GUILayout.Button("Size", EditorStyles.centeredGreyMiniLabel))
        {
            myTarget.showSize = !myTarget.showSize;
        }
        if (myTarget.showSize)
        {
            myTarget.sizeChilds = EditorGUILayout.Vector3Field("Size : ", myTarget.sizeChilds);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Copy First Child Size"))
            {
                myTarget.sizeChilds = myTarget.childs[0].localScale;
            }
            if (GUILayout.Button("Update Child Size"))
            {
                myTarget.UpdateSize();
            }
            EditorGUILayout.EndHorizontal();

        }
        GUILayout.EndVertical();
        #endregion
        #region Separation
        GUILayout.BeginVertical(EditorStyles.helpBox);
        if (GUILayout.Button("Separation", EditorStyles.centeredGreyMiniLabel))
        {
            myTarget.showSeparation = !myTarget.showSeparation;
        }
        if (myTarget.showSeparation)
        {
            myTarget.offsetSeparation = EditorGUILayout.Vector3Field("Offset : ", myTarget.offsetSeparation);
            myTarget.separation = EditorGUILayout.Vector3Field("Separation : ", myTarget.separation);
            if (GUILayout.Button("Update Child Position"))
            {
                myTarget.Separation();
            }
        }
        GUILayout.EndVertical();
        #endregion
    }
}



