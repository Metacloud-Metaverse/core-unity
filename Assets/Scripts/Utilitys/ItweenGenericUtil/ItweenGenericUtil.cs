using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ItweenUtil",menuName ="ScriptableObject/ItweenUtils")]
public class ItweenGenericUtil : ScriptableObject
{
    public Vector3 vector;
    public float time;
    public bool isLocal;
    public iTween.EaseType ease;
    public iTween.LoopType loop;
    public string callbackName;
    
    public Hashtable GetHas()
    {
        var hash =iTween.Hash("x",vector.x,"y",vector.y,"z",vector.z,"time",time,"easeType"
            ,ease,"loopType",loop
            ,"isLocal",isLocal
            );
        if (callbackName != String.Empty)
        {
            hash.Add("onComplete",callbackName);
        }
        return hash;
    }
}
