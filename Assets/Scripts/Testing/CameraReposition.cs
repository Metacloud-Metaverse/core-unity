using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReposition : MonoBehaviour
{
    private Vector3 _initialPos;
    private Quaternion _initialRot;
    public MonoBehaviour controller;

    void Start()
    {
        _initialPos = transform.position;
        _initialRot = transform.rotation;
    }

    public void RestartCameraPosition()
    {
        controller.enabled = false;
        transform.position = _initialPos;
        transform.rotation = _initialRot;
        controller.enabled = true;
    }

}
