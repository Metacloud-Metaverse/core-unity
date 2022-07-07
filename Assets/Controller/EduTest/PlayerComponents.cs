using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerComponents 
{
    public CharacterController controller;
    public Rigidbody rb;
    public Camera camera;
    public Transform root;
    public GameObject playerObject;
    private Animation _anim;
    public Animation anim
    {
        get
        {
            if (_anim == null)
                _anim = playerObject.GetComponentInChildren<Animation>();
            return _anim;
        }
    }
}
