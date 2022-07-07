using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMixamoBones : MonoBehaviour
{
    public string rootBoneName = "Hips";
    void Awake()
    {
        var rootBone = transform.Find(rootBoneName);
        SetMixamoName(rootBone);
    }

    private void SetMixamoName(Transform bone)
    {
        bone.name = $"mixamorig:{bone.name}";
        for (int i = 0; i < bone.childCount; i++)
        {
            SetMixamoName(bone.GetChild(i));
        }
    }
}
