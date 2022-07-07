using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerControllerComponents
{
    private Animation _anim;
    public Rigidbody rb;
    public Animation anim
    {
        get
        {
            if (_anim == null)
                _anim = gameObject.GetComponentInChildren<Animation>();
            return _anim;
        }
    }
    public CapsuleCollider coll;
    public Transform cam;
    public GameObject gameObject;
}
