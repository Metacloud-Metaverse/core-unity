using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GridChildController : MonoBehaviour
{
    public List<Transform> childs = new List<Transform>();
    public Vector3 sizeChilds;
    public Vector3 offsetSeparation;
    public Vector3 separation;

    #region editor Vars
    public int childCount;
    public bool showSize;
    public bool showSeparation;
    #endregion
    [ContextMenu("GetChilds")]
    public void GetChildObjects()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            childs.Add(transform.GetChild(i));
        }
    }
    [ContextMenu("Size")]
    public void UpdateSize()
    {
        foreach (var child in childs)
        {
            child.transform.localScale = sizeChilds;
        }
    }
    [ContextMenu("separation")]
    public void Separation()
    {
        for (int i = 0; i < childs.Count; i++)
        {
            childs[i].transform.localPosition = Vector3.zero;
        }
        var separationInit = Vector3.zero;
        for (int i = 0; i < childs.Count; i++)
        {
            childs[i].transform.localPosition = offsetSeparation+separationInit;
            separationInit += separation;
        }
    }
}
